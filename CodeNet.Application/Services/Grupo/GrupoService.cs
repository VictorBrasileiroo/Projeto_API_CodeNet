using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.Grupo;
using CodeNet.Application.Interfaces.Grupo;
using CodeNet.Application.Validations.GrupoValidator;
using CodeNet.Core.IRepositories;
using CodeNet.Core.Models;
using FluentValidation;

namespace CodeNet.Application.Services.Grupo
{
    public class GrupoService : IGrupoService
    {
        private readonly IGrupoMembroRepository _repositoryMembro;
        private readonly IGrupoRepository _repository;

        public GrupoService(IGrupoMembroRepository repositoryMembro, IGrupoRepository repository)
        {
            _repositoryMembro = repositoryMembro;
            _repository = repository;
        }

        public async Task<GrupoModel> CriarGrupo(GrupoDto dto, Guid idUser)
        {
            var validator = new GrupoValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                var erros = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(erros);
            }

            if (await _repository.ExisteTitulo(dto.Titulo)) throw new InvalidOperationException("Esse título já existe");

            var grupo = new GrupoModel()
            {
                Id = Guid.NewGuid(),
                Titulo = dto.Titulo!.Trim(),
                Descricao = dto.Descricao,
                CriadoEm = DateTime.UtcNow,
            };

            var grupoCriado = await _repository.CreateGrupo(grupo);

            var membroAdm = new GrupoMembroModel()
            {
                Id = Guid.NewGuid(),
                IdUser = idUser,
                IdGrupo = grupoCriado.Id,
                EntrouEm = DateTime.UtcNow,
                Papel = "Admin"
            };

            await _repositoryMembro.CreateMembro(membroAdm);
            return grupoCriado;
        }

        public async Task<GrupoModel> EditarGrupo(GrupoDto dto, Guid idUser, Guid idGrupo)
        {
            var grupo = await _repository.GetById(idGrupo);
            if (grupo == null) throw new KeyNotFoundException("Esse grupo não existe!");

            var membro = await _repositoryMembro.GetMembro(idUser, idGrupo);
            if (membro == null) throw new UnauthorizedAccessException("Você não participa deste grupo.");

            if (membro.Papel != "Admin") throw new UnauthorizedAccessException("Você não tem permissão para editar o grupo!");

            if (dto.Titulo != grupo.Titulo && await _repository.ExisteTitulo(dto.Titulo)) throw new InvalidOperationException("Esse título já existe");

            var validator = new GrupoEditarValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                var erros = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(erros);
            }

            AtualizarCampoSeMudou(grupo.Titulo, dto.Titulo, novoValor => grupo.Titulo = novoValor);
            AtualizarCampoSeMudou(grupo.Descricao, dto.Descricao, novoValor => grupo.Descricao = novoValor);

            return await _repository.UpdateGrupo(grupo);
        }

        public async Task<GrupoModel> ExcluirGrupo(Guid idGrupo, Guid idUser)
        {
            var grupo = await _repository.GetById(idGrupo);
            if (grupo == null) throw new KeyNotFoundException("Esse grupo não existe!");

            var membro = await _repositoryMembro.GetMembro(idUser, idGrupo);
            if (membro == null) throw new UnauthorizedAccessException("Você não participa deste grupo.");

            if (membro.Papel != "Admin") throw new UnauthorizedAccessException("Você não tem permissão para excluir o grupo!");

            return await _repository.DeleteGrupo(grupo);
        }

        public async Task<List<GrupoModel>> ListarTodos()
        {
            var grupos = await _repository.GetAll();
            if (!grupos.Any()) throw new Exception("Nenhum grupo cadastrado");
            return grupos;
        }

        private void AtualizarCampoSeMudou<T>(T campoAtual, T novoValor, Action<T> atualizarCampo)
        {
            if (novoValor != null && !Equals(campoAtual, novoValor) && !Equals(novoValor, "string"))
            {
                atualizarCampo(novoValor);
            }
        }
    }
}
