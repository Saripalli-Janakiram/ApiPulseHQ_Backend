using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPulseHQ.Application.DTOs.Auth
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
