using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeNet.Core.Models
{
    public class GrupoModel
    {
        public Guid Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<GrupoMembroModel> Membros { get; set; }

        [JsonIgnore]
        public ICollection<MensagemModel> Mensagens { get; set; }
    }
}
