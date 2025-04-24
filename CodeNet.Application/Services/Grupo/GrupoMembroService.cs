using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Interfaces.Grupo;
using CodeNet.Core.IRepositories;
using CodeNet.Core.Models;
using CodeNet.Core.Shared;

namespace CodeNet.Application.Services.Grupo
{
    public class GrupoMembroService : IGrupoMembroService
    {
        private readonly IGrupoMembroRepository _repository;
        private readonly IGrupoRepository _repositoryGrupo;

        public GrupoMembroService(IGrupoMembroRepository repository, IGrupoRepository repositoryGrupo)
        {
            _repository = repository;
            _repositoryGrupo = repositoryGrupo;
        }

        public async Task<bool> EhAdmin(Guid idUser, Guid idGrupo)
        {
            var membro = await _repository.GetMembro(idUser, idGrupo);
            if (membro == null) throw new KeyNotFoundException("Esse usuário não participa do grupo");
            return membro.Papel == "Admin";
        }

        public async Task<bool> EhMembro(Guid idUser, Guid idGrupo)
        {
            var membro = await _repository.GetMembro(idUser, idGrupo);
            if (membro == null) throw new KeyNotFoundException("Esse usuário não participa do grupo");
            return true;
        }

        public async Task<GrupoMembroModel> EntrarNoGrupo(Guid idUser, Guid idGrupo)
        {
            var grupo = await _repositoryGrupo.GetById(idGrupo);
            if (grupo == null) throw new KeyNotFoundException("Esse grupo não existe");

            bool membroParticipa = await _repository.UsuarioParticipa(idUser, idGrupo);
            if (membroParticipa) throw new InvalidOperationException("Usuário já participa desse grupo!");

            var membro = new GrupoMembroModel()
            {
                Id = Guid.NewGuid(),
                IdUser = idUser,
                IdGrupo = idGrupo,
                Papel = "User",
                EntrouEm = DateTime.UtcNow,
            };

            return await _repository.CreateMembro(membro);
        }

        public async Task<List<UserResponseCoreDto>> ListarMembros(Guid idGrupo)
        {
            var grupo = await _repositoryGrupo.GetById(idGrupo);
            if (grupo == null) throw new KeyNotFoundException("Esse grupo não existe");

            var response = await _repository.GetMembrosParaExibir(idGrupo);
            return response;
        }

        public async Task<List<GrupoModel>> ListarMeusGrupos(Guid idUser)
        {
            var grupos = await _repository.GetGruposPorUser(idUser);
            if (!grupos.Any()) throw new Exception("O usuário não está em nenhum grupo");
            return grupos;
        }

        public async Task<GrupoMembroModel> SairDoGrupo(Guid idUser, Guid idGrupo)
        {
            var grupo = await _repositoryGrupo.GetById(idGrupo);
            if (grupo == null) throw new KeyNotFoundException("Esse grupo não existe");

            var membro = await _repository.GetMembro(idUser, idGrupo);
            if (membro == null) throw new InvalidOperationException("Usuário não participa desse grupo!");

            if(membro.Papel == "Admin")
            {
                var membros = await _repository.GetMembrosPorGrupo(grupo.Id);
                var membrosRestantes = membros.Where(m => m.IdUser != idUser);
                if (!membrosRestantes.Any())
                {
                    await _repositoryGrupo.DeleteGrupo(grupo);
                }
                else
                {
                    var novoAdm = membrosRestantes.OrderBy(m => m.EntrouEm).First();
                    novoAdm.Papel = "Admin";
                    await _repository.UpdateMembro(novoAdm);
                }
            }

            return await _repository.DeleteMembro(membro);
        }
    }
}
