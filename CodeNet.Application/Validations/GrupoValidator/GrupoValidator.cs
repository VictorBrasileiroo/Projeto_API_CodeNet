using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto;
using FluentValidation;

namespace CodeNet.Application.Validations.GrupoValidator
{
    public class GrupoValidator : AbstractValidator<GrupoDto>
    {
        public GrupoValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("O título do grupo é obrigatório.")
                .MinimumLength(3).WithMessage("O título deve ter pelo menos 3 caracteres.")
                .Must(t => t != "string").WithMessage("Informe um título válido.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição do grupo é obrigatória.")
                .MinimumLength(5).WithMessage("A descrição deve ter pelo menos 5 caracteres.")
                .Must(d => d != "string").WithMessage("Informe uma descrição válida.");
        }
    }
}
