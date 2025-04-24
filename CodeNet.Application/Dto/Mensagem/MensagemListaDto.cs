using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeNet.Application.Dto.Mensagem
{
    public class MensagemListaDto
    {
        public Guid Id { get; set; }
        public string Comentario { get; set; }
        public DateTime EnviadoEm { get; set; }
        public bool Editado { get; set; }
        public string NomeUsuario { get; set; }
    }
}
