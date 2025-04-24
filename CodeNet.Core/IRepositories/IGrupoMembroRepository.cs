using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Core.Models;
using CodeNet.Core.Shared;

namespace CodeNet.Core.IRepositories
{
    public interface IGrupoMembroRepository
    {
        Task<bool> UsuarioParticipa(Guid idUser, Guid idGrupo);
        Task<GrupoMembroModel> CreateMembro(GrupoMembroModel membro);
        Task<GrupoMembroModel> DeleteMembro(GrupoMembroModel membro);
        Task<List<UserResponseCoreDto>> GetMembrosParaExibir(Guid idGrupo);
        Task<List<GrupoMembroModel>> GetMembrosPorGrupo(Guid idGrupo);
        Task<List<GrupoModel>> GetGruposPorUser(Guid idUser);
        Task<GrupoMembroModel> GetMembro(Guid idUser, Guid idGrupo);
        Task<GrupoMembroModel> UpdateMembro(GrupoMembroModel membro);
    }
}
