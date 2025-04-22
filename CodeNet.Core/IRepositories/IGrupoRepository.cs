using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Core.Models;

namespace CodeNet.Core.IRepositories
{
    public interface IGrupoRepository
    {
        Task<GrupoModel> GetById(Guid id);
        Task<List<GrupoModel>> GetAll();
        Task<GrupoModel> CreateGrupo(GrupoModel grupo);
        Task<GrupoModel> DeleteGrupo(GrupoModel grupo);
        Task<GrupoModel> UpdateGrupo(GrupoModel grupo);
        Task<bool> ExisteTitulo(string titulo);
    }
}
