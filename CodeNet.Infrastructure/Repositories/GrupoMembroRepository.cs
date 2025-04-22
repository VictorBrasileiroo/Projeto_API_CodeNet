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
    public class GrupoMembroRepository : IGrupoMembroRepository
    {
        private readonly AppDbContext _context;

        public GrupoMembroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GrupoMembroModel> CreateMembro(GrupoMembroModel membro)
        {
            _context.Add(membro);
            await _context.SaveChangesAsync();
            return membro;
        }

        public async Task<GrupoMembroModel> DeleteMembro(GrupoMembroModel membro)
        {
            _context.Remove(membro);
            await _context.SaveChangesAsync();
            return membro;
        }

        public async Task<List<GrupoMembroModel>> GetGruposPorUser(Guid idUser)
        {
            return await _context.GrupoMembros.Where(gm => gm.IdUser == idUser).Include(gm => gm.Grupo).ToListAsync();
        }

        public async Task<GrupoMembroModel> GetMembro(Guid idUser, Guid idGrupo) => await _context.GrupoMembros.FirstOrDefaultAsync(gm => gm.IdUser == idUser && gm.IdGrupo == idGrupo);

        public async Task<List<GrupoMembroModel>> GetMembrosPorGrupo(Guid idGrupo)
        {
            return await _context.GrupoMembros.Where(gm => gm.IdGrupo == idGrupo).Include(gm => gm.User).ToListAsync();
        }

        public async Task<bool> UsuarioParticipa(Guid idUser, Guid idGrupo) => await _context.GrupoMembros.AnyAsync(gm => gm.IdUser == idUser && gm.IdGrupo == idGrupo);
    }
}
