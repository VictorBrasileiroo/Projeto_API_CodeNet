using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Core.Models;

namespace CodeNet.Core.IRepositories
{
    public interface IGrupoMembroRepository
    {
        Task<bool> UsuarioParticipa(Guid idUser, Guid idGrupo);
        Task<GrupoMembroModel> CreateMembro(GrupoMembroModel membro);
        Task<GrupoMembroModel> DeleteMembro(GrupoMembroModel membro);
        Task<List<GrupoMembroModel>> GetMembrosPorGrupo(Guid idGrupo);
        Task<List<GrupoMembroModel>> GetGruposPorUser(Guid idUser);
        Task<GrupoMembroModel> GetMembro(Guid idUser, Guid idGrupo);
    }
}
