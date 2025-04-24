using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeNet.Application.Dto.Mensagem;
using CodeNet.Application.Interfaces.Mensagem;
using CodeNet.Application.Validations.MensagemValidator;
using CodeNet.Core.IRepositories;
using CodeNet.Core.Models;
using FluentValidation;

namespace CodeNet.Application.Services.Mensagem
{
    public class MensagemService : IMensagemService
    {
        private readonly IGrupoMembroRepository _repositoryMembro;
        private readonly IMensagemRepository _repository;
        private readonly IGrupoRepository _repositoryGrupo;
        private readonly IUserRepository _repositoryUser;

        public MensagemService(IGrupoMembroRepository repositoryMembro, IMensagemRepository repository, IGrupoRepository repositoryGrupo, IUserRepository repositoryUser)
        {
            _repositoryMembro = repositoryMembro;
            _repository = repository;
            _repositoryGrupo = repositoryGrupo;
            _repositoryUser = repositoryUser;
        }

        public async Task<MensagemModel> EditarMensagem(MensagemDto dto, Guid idMensagem, Guid idUser)
        {
            var validator = new MensagemEditarValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                var erros = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(erros);
            }

            var user = await _repositoryUser.GetById(idUser);
            if (user == null) throw new KeyNotFoundException("Esse usuário não existe!");

            var mensagem = await _repository.GetById(idMensagem);
            if (mensagem == null) throw new KeyNotFoundException("Mensagem não encontrada");

            var membro = await _repositoryMembro.GetMembro(idUser, mensagem.IdGrupo);
            if (membro == null) throw new KeyNotFoundException("Você não faz parte do grupo");

            if (mensagem.IdUser != membro.IdUser) throw new UnauthorizedAccessException("Você não tem permissão para editar esta mensagem");

            AtualizarCampoSeMudou(mensagem.Comentario, dto.Comentario, novoValor => mensagem.Comentario = novoValor);
            mensagem.Editado = true;

            return await _repository.UpdateMensage(mensagem);
        }

        public async Task<MensagemModel> EnviarMensagem(MensagemDto dto, Guid idUser, Guid idGrupo)
        {
            var validator = new MensagemCriarValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                var erros = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(erros);
            }

            var grupo = await _repositoryGrupo.GetById(idGrupo);
            if (grupo == null) throw new KeyNotFoundException("Esse grupo não existe");

            var user = await _repositoryUser.GetById(idUser);
            if (user == null) throw new KeyNotFoundException("Esse usuário não existe!");

            var membro = await _repositoryMembro.GetMembro(idUser, idGrupo);
            if (membro == null) throw new KeyNotFoundException("Esse usuário não pertece ao grupo");

            var mensagem = new MensagemModel()
            {
                Id = Guid.NewGuid(),
                Comentario = dto.Comentario,
                EnviadoEm = DateTime.UtcNow,
                IdUser = idUser,
                IdGrupo = idGrupo,
                Editado = false
            };

            return await _repository.CreateMensage(mensagem);
        }

        public async Task<MensagemModel> ExcluirMensagem(Guid idMensagem, Guid idUser)
        {
           var mensagem = await _repository.GetById(idMensagem);
           if (mensagem == null) throw new KeyNotFoundException("Mensagem não encontrada");

           var membro = await _repositoryMembro.GetMembro(idUser, mensagem.IdGrupo);
           if (membro == null) throw new KeyNotFoundException("Você não faz parte do grupo");

            var user = await _repositoryUser.GetById(idUser);

           if (mensagem.IdUser != user.Id && membro.Papel != "Admin") throw new UnauthorizedAccessException("Você não tem permissão para excluir esta mensagem");

           return await _repository.RemoveMensage(mensagem);

        }

        public async Task<List<MensagemListaDto>> ListarMensagensDoGrupo(Guid idGrupo)
        {
            var grupo = await _repositoryGrupo.GetById(idGrupo);
            if(grupo == null) throw new KeyNotFoundException("Grupo não encontrado");

            var mensagens = await _repository.GetAllByGrupo(idGrupo);
            if (!mensagens.Any()) throw new KeyNotFoundException("Ainda não há mensagens neste grupo");

            var response = mensagens.Select(m => new MensagemListaDto()
            {
                Id = m.Id,
                NomeUsuario = m.User?.Nome,
                Comentario = m.Comentario,
                EnviadoEm = m.EnviadoEm,
                Editado = m.Editado,
            }).ToList();

            return response;
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
