using LudusApp.Domain.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Domain.Services
{
    public class UsuarioDomainService
    {
        private readonly UserManager<Usuario> _userManager;

        public UsuarioDomainService(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> ValidarDuplicidadesAsync(string email, string username, string cpf)
        {
            var duplicados = await _userManager.Users
                .Where(u => u.Email == email || u.UserName == username || u.Cpf == cpf)
                .ToListAsync();

            return duplicados.Any();
        }
    }
}