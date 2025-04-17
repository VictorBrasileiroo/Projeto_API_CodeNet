using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.User;
using CodeNet.Core.Models;

namespace CodeNet.Application.Interfaces.User
{
    public interface IUserService
    {
        Task<List<UserModel>> ListarTodos();
        Task<UserModel> RemoverUser(string password, Guid id); 
        Task<UserModel> RemoverUserAdm(Guid id); 
        Task<UserModel> AlterarSenha(string password, string newPassword, Guid id); 
        Task<UserModel> EditarUser(UserDto dto, Guid id);
        Task<UserDto> BuscarPorNome(string nome); 
    }
}
