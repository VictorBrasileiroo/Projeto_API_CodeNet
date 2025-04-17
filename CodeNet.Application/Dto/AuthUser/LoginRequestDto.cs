using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeNet.Application.Dto.AuthUser
{
    public record LoginRequestDto(string Email, string Password);
}
