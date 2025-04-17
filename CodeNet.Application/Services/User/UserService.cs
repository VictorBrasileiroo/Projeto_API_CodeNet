using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.User;
using CodeNet.Application.Interfaces.User;
using CodeNet.Application.Validations.UserValidator;
using CodeNet.Core.IRepositories;
using CodeNet.Core.Models;
using FluentValidation;

namespace CodeNet.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserModel> AlterarSenha(string password,string newPassword, Guid id)
        {
            var user = await _repository.GetById(id);
            if (user == null) throw new Exception("Usuário não encontrado!");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) throw new UnauthorizedAccessException("Senha incorreta");

            var validator = new SenhaValidator();
            var result = validator.Validate(newPassword);

            if (!result.IsValid)
            {
                var erros = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(erros);
            }

            var novoHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            user.PasswordHash = novoHash;

            var response = await _repository.UpdateUser(user);
            return response;
        }

        public async Task<UserDto> BuscarPorNome(string nome)
        {
            var user = await _repository.GetByName(nome);
            var response = new UserDto()
            {
                Nome = user.Nome,
                Genero = user.Genero,
                StackPrincipal = user.StackPrincipal,
            };
            return response;
        }

        public async Task<UserModel> EditarUser(UserDto dto, Guid id)
        {
            var user = await _repository.GetById(id);
            if (user == null) throw new Exception("Usuário não encontrado!");

            var validator = new UserValidator();
            var result = validator.Validate(dto);

            if (!result.IsValid)
            {
                var erros = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(erros);
            }

            AtualizarCampoSeMudou(user.Nome, dto.Nome, novoValor => user.Nome = novoValor);
            AtualizarCampoSeMudou(user.Genero, dto.Genero, novoValor => user.Genero = novoValor);
            AtualizarCampoSeMudou(user.StackPrincipal, dto.StackPrincipal, novoValor => user.StackPrincipal = novoValor);

            var response = await _repository.UpdateUser(user);
            return response;
        }

        public async Task<List<UserModel>> ListarTodos()
        {
            var response = await _repository.GetAll();
            return response;
        }

        public async Task<UserModel> RemoverUser(string password, Guid id)
        {
            var user = await _repository.GetById(id);
            if (user == null) throw new Exception("Usuário não encontrado!");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) throw new UnauthorizedAccessException("Senha incorreta");

            var response = await _repository.DeleteUser(user);
            return response;
        }

        public async Task<UserModel> RemoverUserAdm(Guid id)
        {
            var user = await _repository.GetById(id);
            if (user == null) throw new Exception("Usuário não encontrado!");

            var response = await _repository.DeleteUser(user);
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
