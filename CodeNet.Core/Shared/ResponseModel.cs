using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeNet.Core.Shared
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Dados { get; set; }

        public ResponseModel(bool sucesso, string mensagem, T dados)
        {
            this.Success = sucesso;
            this.Mensagem = mensagem;
            this.Dados = dados;
        }
    }
}
