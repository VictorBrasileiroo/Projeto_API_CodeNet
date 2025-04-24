using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Core.Models;
using CodeNet.Core.Shared;

namespace CodeNet.Application.Interfaces.GrupoMembro
{
    public interface IGrupoMembroService
    {
        Task<GrupoMembroModel> EntrarNoGrupo(Guid idUser, Guid idGrupo);
        Task<GrupoMembroModel> SairDoGrupo(Guid idUser, Guid idGrupo);
        Task<List<UserResponseCoreDto>> ListarMembros(Guid idGrupo);
        Task<List<GrupoModel>> ListarMeusGrupos(Guid idUser);
        Task<bool> EhAdmin(Guid idUser, Guid idGrupo);
        Task<bool> EhMembro(Guid idUser, Guid idGrupo);
    }
}
