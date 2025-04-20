using Microsoft.AspNetCore.Authorization;

namespace LudusApp.Application.Authorization;

public class EmpresaRequirementHandler : AuthorizationHandler<EmpresaRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmpresaRequirement requirement)
    {
        // Busca o tenant_id a partir dos claims do usuário
        var tenantIdClaim = context.User.FindFirst("tenant_id")?.Value;

        if (string.IsNullOrEmpty(tenantIdClaim))
        {
            // Falha a autorização caso não tenha o tenant_id
            context.Fail(); 
        }
        else
        {
            // Sucesso se o tenant_id existir
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
