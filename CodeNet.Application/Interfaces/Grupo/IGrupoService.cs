using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.Grupo;
using CodeNet.Core.IRepositories;
using CodeNet.Core.Models;

namespace CodeNet.Application.Interfaces.Grupo
{
    public interface IGrupoService
    {
        Task<GrupoModel> CriarGrupo(GrupoDto dto, Guid idUser);
        Task<GrupoModel> ExcluirGrupo(Guid idGrupo, Guid idUser);
        Task<GrupoModel> EditarGrupo(GrupoDto dto, Guid idUser, Guid idGrupo);
        Task<List<GrupoModel>> ListarTodos();
    }
}
