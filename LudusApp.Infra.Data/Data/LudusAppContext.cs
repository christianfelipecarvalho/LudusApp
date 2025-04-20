using LudusApp.Domain.Empresas;
using LudusApp.Domain.Entities.Emails;
using LudusApp.Domain.Entities.Evento;
using LudusApp.Domain.Entities.Local;
using LudusApp.Domain.Entities.Localidades;
using LudusApp.Domain.Entities.Localidades.Cidade;
using LudusApp.Domain.Entities.Localidades.Estado;
using LudusApp.Domain.Entities.VinculosUsuarioEmpresa;
using LudusApp.Domain.TemaSettings;
using LudusApp.Domain.Usuarios;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Infra.Data;

public class LudusAppContext : IdentityDbContext<Usuario>
{
    public LudusAppContext(DbContextOptions<LudusAppContext> opts) : base(opts) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Estado> Estados { get; set; }
    public DbSet<Cidade> Cidades { get; set; }
    public DbSet<Bairro> Bairros { get; set; }
    public DbSet<Local> Locais { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Tema> Temas { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<ConfiguracaoEmail> ConfiguracoesEmail { get; set; }
    public DbSet<TemplateEmail> TemplatesEmail { get; set; }
    public DbSet<UsuarioEmpresa> UsuariosEmpresas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Cpf)
            .IsUnique();

        // Configuração da relação intermediária UsuarioEmpresa
        modelBuilder.Entity<UsuarioEmpresa>()
            .HasKey(ue => new { ue.EmpresaId, ue.UsuarioId }); 

        modelBuilder.Entity<UsuarioEmpresa>()
            .HasOne(ue => ue.Empresa)
            .WithMany(e => e.UsuariosEmpresas)
            .HasForeignKey(ue => ue.EmpresaId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<UsuarioEmpresa>()
            .HasOne(ue => ue.Usuario)
            .WithMany(u => u.UsuariosEmpresas)
            .HasForeignKey(ue => ue.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Tema>()
            .HasOne(t => t.Usuario)
            .WithOne(u => u.Tema)
            .HasForeignKey<Tema>(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Email>()
               .Property(e => e.Destinatario)
               .IsRequired();
        modelBuilder.Entity<TemplateEmail>().Property(t => t.Tipo).IsRequired();
        
        modelBuilder.Entity<Local>()
            .HasOne(l => l.Cidade)
            .WithMany(c => c.Locais)
            .HasForeignKey(l => l.CidadeId)
            .OnDelete(DeleteBehavior.Restrict); 
        
        modelBuilder.Entity<Evento>()
            .HasOne(e => e.Local)
            .WithMany(l => l.Eventos) // para Local
            .HasForeignKey(e => e.IdLocal)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Evento>()
            .HasOne(e => e.Usuario)
            .WithMany(u => u.Eventos) // para Usuario
            .HasForeignKey(e => e.IdUsuario)
            .OnDelete(DeleteBehavior.SetNull);

    }

}
