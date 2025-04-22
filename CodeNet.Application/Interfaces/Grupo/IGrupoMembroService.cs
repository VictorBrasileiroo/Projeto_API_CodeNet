using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Core.Models;

namespace CodeNet.Application.Interfaces.Grupo
{
    public interface IGrupoMembroService
    {
        Task<GrupoMembroModel> EntrarNoGrupo(Guid idUser, Guid idGrupo);
        Task<GrupoMembroModel> SairDoGrupo(Guid idUser, Guid idGrupo);
        Task<List<GrupoMembroModel>> ListarMembros(Guid idGrupo);
        Task<List<GrupoMembroModel>> ListarMeusGrupos(Guid idUser);
        Task<bool> EhAdmin(Guid idUser, Guid idGrupo);
        Task<bool> EhMembro(Guid idUser, Guid idGrupo);
    }
}
