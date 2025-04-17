using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CodeNet.Core.Shared
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idClaim is null || !Guid.TryParse(idClaim, out var userId))
                throw new UnauthorizedAccessException("Usuário não autenticado ou ID inválido");

            return userId;
        }

        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            return email ?? throw new UnauthorizedAccessException("Email do usuário não encontrado");
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            return role ?? throw new UnauthorizedAccessException("Role do usuário não encontrada");
        }

        public static string GetUserGenero(this ClaimsPrincipal user)
        {
            var genero = user.FindFirst(ClaimTypes.Gender)?.Value;
            return genero ?? throw new UnauthorizedAccessException("Gênero do usuário não encontrado");
        }

        public static bool IsInRole(this ClaimsPrincipal user, string role)
        {
            return user.IsInRole(role);
        }
    }
}
