using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Interfaces.Grupo;
using CodeNet.Core.IRepositories;
using CodeNet.Core.Models;

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

        //USA NO CONTROLE NA HORA DE EDITAR O GRUPO --< GRUPO SERVICE
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

        public async Task<List<GrupoMembroModel>> ListarMembros(Guid idGrupo)
        {
            var grupo = await _repositoryGrupo.GetById(idGrupo);
            if (grupo == null) throw new KeyNotFoundException("Esse grupo não existe");

            var response = await _repository.GetMembrosPorGrupo(idGrupo);
            return response;
        }

        public async Task<List<GrupoMembroModel>> ListarMeusGrupos(Guid idUser)
        {
            var grupos = await _repository.GetGruposPorUser(idUser);
            if (grupos == null) throw new Exception("O usuário não está em nenhum grupo");
            return grupos;
        }

        public async Task<GrupoMembroModel> SairDoGrupo(Guid idUser, Guid idGrupo)
        {
            var grupo = await _repositoryGrupo.GetById(idGrupo);
            if (grupo == null) throw new KeyNotFoundException("Esse grupo não existe");

            var membro = await _repository.GetMembro(idUser, idGrupo);
            if (membro == null) throw new InvalidOperationException("Usuário não participa desse grupo!");

            return await _repository.DeleteMembro(membro);
        }
    }
}
