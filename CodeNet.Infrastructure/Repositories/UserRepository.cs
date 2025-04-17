using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Core.IRepositories;
using CodeNet.Core.Models;
using CodeNet.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CodeNet.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) => _context = context;

        public async Task<UserModel> CreateUser(UserModel user)
        {
           _context.Add(user);
           await _context.SaveChangesAsync();
           return user;
        }

        public async Task<UserModel> DeleteUser(UserModel user)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> EmailExist(string email) => await _context.Usuarios.AnyAsync(u => u.Email == email);

        public async Task<List<UserModel>> GetAll() => await _context.Usuarios.ToListAsync();

        public async Task<UserModel> GetByEmail(string email) => await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<UserModel> GetById(Guid id) => await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<UserModel> UpdateUser(UserModel user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
