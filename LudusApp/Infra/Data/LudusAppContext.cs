using LudusApp.Domain.Usuarios;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Infra.Data;

public class LudusAppContext : IdentityDbContext<Usuario>
{
    public LudusAppContext(DbContextOptions<LudusAppContext> opts) : base(opts) { }

    public DbSet<Usuario> Usuarios { get; set; }
}
