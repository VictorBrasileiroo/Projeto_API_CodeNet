using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeNet.Application.Dto.AuthUser
{
    public record RegisterRequestDto(string Email, string Password, string Nome, string Role, DateTime Criacao, string Genero, string Stack);
}
