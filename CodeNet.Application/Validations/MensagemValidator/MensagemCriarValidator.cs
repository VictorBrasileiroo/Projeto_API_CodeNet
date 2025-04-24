using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.Mensagem;
using FluentValidation;

namespace CodeNet.Application.Validations.MensagemValidator
{
    internal class MensagemCriarValidator : AbstractValidator<MensagemDto>
    {
        public MensagemCriarValidator()
        {
            RuleFor(x => x.Comentario)
                .NotEmpty().WithMessage("O comentário não pode estar vazio.")
                .When(x => x.Comentario != null || x.Comentario != "string")
                .MinimumLength(3).WithMessage("O comentário deve ter no mínimo 3 caracteres.")
                .MaximumLength(500).WithMessage("O comentário deve ter no máximo 500 caracteres.");
        }
    }
}
