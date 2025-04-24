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
    public class MensagemRepository : IMensagemRepository
    {
        private readonly AppDbContext _context;

        public MensagemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MensagemModel> CreateMensage(MensagemModel mensage)
        {
            _context.Add(mensage);
            await _context.SaveChangesAsync();
            return mensage;
        }

        public async Task<List<MensagemModel>> GetAllByGrupo(Guid idGrupo) => await _context.Mensagens.Where(m => m.IdGrupo == idGrupo).ToListAsync();

        public async Task<MensagemModel> GetById(Guid idMensage) => await _context.Mensagens.FirstOrDefaultAsync(m => m.Id == idMensage);

        public async Task<MensagemModel> RemoveMensage(MensagemModel mensage)
        {
            _context.Remove(mensage);
            await _context.SaveChangesAsync();
            return mensage;
        }

        public async Task<MensagemModel> UpdateMensage(MensagemModel mensage)
        {
            _context.Update(mensage);
            await _context.SaveChangesAsync();
            return mensage;
        }
    }
}
