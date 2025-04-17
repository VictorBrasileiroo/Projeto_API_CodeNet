using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.AuthUser;
using CodeNet.Core.Models;
using FluentValidation;

namespace CodeNet.Application.Validation.UserValidator
{
    public class UserValidator : AbstractValidator<RegisterRequestDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .Must(n => n != "string").WithMessage("Digite um nome válido")
                .EmailAddress().WithMessage("E-mail inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .Must(n => n != "string").WithMessage("Digite um nome válido")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Must(n => n != "string").WithMessage("Digite um nome válido")
                .MinimumLength(2).WithMessage("O nome deve ter pelo menos 2 caracteres.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("O papel do usuário é obrigatório.")
                .Must(n => n != "string").WithMessage("Digite um nome válido")
                .Must(r => r == "Admin" || r == "User" || r == "Dev")
                .WithMessage("Role inválido. Use: Admin, User ou Dev.");

            RuleFor(x => x.Criacao)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data de criação não pode ser futura.");

            RuleFor(x => x.Genero)
                .NotEmpty().WithMessage("O gênero é obrigatório.")
                .Must(n => n != "string").WithMessage("Digite um nome válido");

            RuleFor(x => x.Stack)
                .NotEmpty().WithMessage("A stack principal é obrigatória.")
                .Must(n => n != "string").WithMessage("Digite um nome válido");
        }
    }
}
