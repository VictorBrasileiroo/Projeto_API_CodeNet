using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeNet.Core.Models;

namespace CodeNet.Core.IRepositories
{
    public interface IMensagemRepository
    {
        Task<List<MensagemModel>> GetAllByGrupo(Guid idGrupo);
        Task<MensagemModel> GetById(Guid idMensage);
        Task<MensagemModel> CreateMensage(MensagemModel mensage);
        Task<MensagemModel> RemoveMensage(MensagemModel mensage);
        Task<MensagemModel> UpdateMensage(MensagemModel mensage);
    }
}
