using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Application.Dto.Mensagem;
using CodeNet.Core.Models;

namespace CodeNet.Application.Interfaces.Mensagem
{
    public interface IMensagemService
    {
        Task<MensagemModel> EnviarMensagem(MensagemDto dto, Guid idUser, Guid idGrupo);
        Task<MensagemModel> EditarMensagem(MensagemDto dto, Guid idMensagem, Guid idUser);
        Task<MensagemModel> ExcluirMensagem(Guid idMensagem, Guid idUser);
        Task<List<MensagemListaDto>> ListarMensagensDoGrupo(Guid idGrupo);
    }
}
