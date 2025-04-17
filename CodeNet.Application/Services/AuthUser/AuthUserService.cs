using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using CodeNet.Application.Dto.AuthUser;
using CodeNet.Application.Interfaces.AuthUser;
using CodeNet.Application.Validation.UserValidator;
using CodeNet.Core.IRepositories;
using CodeNet.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CodeNet.Application.Services.AuthUser
{
    public class AuthUserService : IAuthUserService
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;

        public AuthUserService(IUserRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public Task<string> GerarToken(UserModel user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Gender, user.Genero)
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpireMinutes"]!)),
                signingCredentials: creds
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<UserModel> Register(RegisterRequestDto dto)
        {
            if (await _repository.EmailExist(dto.Email)) throw new Exception("Email já cadastrado!");

            var validator = new UserValidator();
            var result = validator.Validate(dto);

            if(!result.IsValid)
            {
                var erros = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(erros);
            }

            var user = new UserModel()
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Nome = dto.Nome,
                Genero = dto.Genero,
                Role = dto.Role,
                StackPrincipal = dto.Stack,
                CriadoEm = DateTime.UtcNow
            };

            var response = await _repository.CreateUser(user);
            return response;
        }

        public async Task<UserModel> ValidacaoCredenciais(string email, string password)
        {
            var user = await _repository.GetByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) throw new Exception("Email ou senha inválido");
            return user;
        }
    }
}
