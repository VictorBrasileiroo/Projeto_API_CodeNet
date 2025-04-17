using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.User;
using FluentValidation;

namespace CodeNet.Application.Validations.UserValidator
{
    public class SenhaValidator : AbstractValidator<string>
    {
        public SenhaValidator()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .Must(n => n != "string").WithMessage("Digite um nome válido")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
}
