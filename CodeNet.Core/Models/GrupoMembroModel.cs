using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeNet.Core.Models
{
    public class GrupoMembroModel
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdGrupo { get; set; }
        public DateTime EntrouEm { get; set; } = DateTime.UtcNow;
        public string? Papel { get; set; }

        [JsonIgnore]
        public UserModel User { get; set; }

        [JsonIgnore]
        public GrupoModel Grupo { get; set; }
    }
}
