using System.ComponentModel;

namespace LudusApp.Domain.Enums;

public enum EnumStatusEmpresa
{
    Ativa,
    Inativa,
    Suspensa
}
public enum EnumPapelUsuarioEmpresa
{
    Admin,
    Funcionario
}
public enum EnumStatusEmail
{
    [Description("ENVIADO")]
    ENVIADO,
    [Description("NAOENVIADO")]
    NAOENVIADO,
    [Description("ERROR")]
    ERROR
}
