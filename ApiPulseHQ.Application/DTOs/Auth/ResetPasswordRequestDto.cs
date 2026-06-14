using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPulseHQ.Application.DTOs.Auth
{
    public class ResetPasswordRequestDto
    {
        public string Token { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
