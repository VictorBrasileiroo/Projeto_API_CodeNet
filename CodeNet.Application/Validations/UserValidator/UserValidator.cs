using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.User;
using FluentValidation;

namespace CodeNet.Application.Validations.UserValidator
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator() 
        {
            RuleFor(x => x.Nome)
               .MinimumLength(2).WithMessage("O nome deve ter pelo menos 2 caracteres.")
               .When(x => !string.IsNullOrWhiteSpace(x.Nome));

            RuleFor(x => x.Genero)
                .MinimumLength(3).WithMessage("Informe um gênero válido.")
                .When(x => !string.IsNullOrWhiteSpace(x.Genero));

            RuleFor(x => x.StackPrincipal)
                .MinimumLength(2).WithMessage("A stack deve ter pelo menos 2 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.StackPrincipal));
        }
    }
}
