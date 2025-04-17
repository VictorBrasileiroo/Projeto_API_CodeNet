using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeNet.Core.Models
{
    public class MensagemModel
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdGrupo { get; set; }
        public string? Comentario { get; set; }
        public DateTime EnviadoEm { get; set; } = DateTime.UtcNow;
        public bool Editado { get; set; } = false;

        [JsonIgnore]
        public UserModel User { get; set; }
        
        [JsonIgnore]
        public GrupoModel Grupo { get; set; }
    }
}
