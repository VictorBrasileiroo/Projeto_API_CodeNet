using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeNet.Application.Dto.User
{
    public class UserDto
    {
        public string? Nome { get; set; }
        public string? Genero { get; set; }
        public string? StackPrincipal { get; set; }
    }
}
