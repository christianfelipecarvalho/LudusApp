using System.ComponentModel;

namespace LudusApp.Domain.Enums;

public enum EnumStatusEmpresa
{
    [Description("Ativa")] Ativa,
    [Description("Inativa")] Inativa,
    [Description("Suspensa")] Suspensa
}

public enum EnumPapelUsuarioEmpresa
{
    Admin,
    Funcionario
}

public enum EnumStatusEmail
{
    [Description("ENVIADO")] ENVIADO,
    [Description("NAOENVIADO")] NAOENVIADO,
    [Description("ERROR")] ERROR
}

public enum EnumStatusLocal
{
    [Description("Inativo")] Inativo = 0,

    [Description("Ativo")] Ativo = 1
}

[Flags]
public enum DiasDaSemana
{
    Nenhum = 0,
    Domingo = 1,
    Segunda = 2,
    Terça = 4,
    Quarta = 8,
    Quinta = 16,
    Sexta = 32,
    Sabado = 64
}

public enum EnumStatusEvento
{
    [Description("Cancelado")] Cancelado = 0,

    [Description("Aguardando Pagamento")] AguardandoPagto = 1,

    [Description("Pago")] Pago = 2
}