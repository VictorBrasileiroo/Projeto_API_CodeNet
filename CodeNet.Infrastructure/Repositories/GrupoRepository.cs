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
    public class GrupoRepository : IGrupoRepository
    {
        private readonly AppDbContext _context;

        public GrupoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GrupoModel> CreateGrupo(GrupoModel grupo)
        {
            _context.Add(grupo);
            await _context.SaveChangesAsync();
            return grupo;
        }

        public async Task<GrupoModel> DeleteGrupo(GrupoModel grupo)
        {
            _context.Remove(grupo);
            await _context.SaveChangesAsync();
            return grupo;
        }

        public async Task<bool> ExisteTitulo(string titulo) => await _context.Grupos.AnyAsync(g => g.Titulo.ToLower().Trim() == titulo.ToLower().Trim());

        public async Task<List<GrupoModel>> GetAll() => await _context.Grupos.ToListAsync();

        public async Task<GrupoModel> GetById(Guid id) => await _context.Grupos.FirstOrDefaultAsync(g => g.Id == id);

        public async Task<GrupoModel> UpdateGrupo(GrupoModel grupo)
        {
            _context.Update(grupo);
            await _context.SaveChangesAsync();
            return grupo;
        }
    }
}
