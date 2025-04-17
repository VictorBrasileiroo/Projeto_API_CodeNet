using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeNet.Core.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }

        [JsonIgnore]
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public  string? Genero { get; set; }
        public  string? StackPrincipal { get; set; }

        [JsonIgnore]
        public ICollection<GrupoMembroModel> Grupos { get; set; }

        [JsonIgnore]
        public ICollection<MensagemModel> Mensagens { get; set; }
    }
}
