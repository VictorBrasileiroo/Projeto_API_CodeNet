using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Core.Models;

namespace CodeNet.Core.IRepositories
{
    public interface IUserRepository
    {
        Task<UserModel> GetByEmail(string email);
        Task<UserModel> GetById(Guid id);
        Task<UserModel> GetByName(string nome);
        Task<List<UserModel>> GetAll();
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModel> DeleteUser(UserModel user);
        Task<UserModel> UpdateUser(UserModel user);
        Task<bool> EmailExist(string email);
    }
}
