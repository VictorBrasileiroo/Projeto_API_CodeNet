using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.AuthUser;
using CodeNet.Core.Models;

namespace CodeNet.Application.Interfaces.AuthUser
{
    public interface IAuthUserService
    {
        Task<UserModel> ValidacaoCredenciais(string email, string password);
        Task<UserModel> Register(RegisterRequestDto dto);
        Task<string> GerarToken(UserModel user);
    }
}
