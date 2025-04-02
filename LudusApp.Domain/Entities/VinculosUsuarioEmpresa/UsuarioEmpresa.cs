using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LudusApp.Domain.Empresas;
using LudusApp.Domain.Enums;
using LudusApp.Domain.Usuarios;

namespace LudusApp.Domain.Entities.VinculosUsuarioEmpresa;

public class UsuarioEmpresa
{
    public Guid EmpresaId { get; set; }
    public Empresa Empresa { get; set; }

    public string UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public EnumPapelUsuarioEmpresa Papel { get; set; } // Permissões como Admin ou Funcionario ou algo especifico
    public DateTime DataVinculo { get; set; } // Data de associação do usuário à empresa
    public DateTime DataUltimaAlteracao { get; set; }
    public Guid? TenantId { get; set; }
}
