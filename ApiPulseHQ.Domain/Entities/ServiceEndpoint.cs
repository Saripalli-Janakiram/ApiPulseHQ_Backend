using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPulseHQ.Domain.Entities
{
    public class ServiceEndpoint
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = default!;
        public string Url { get; set; } = default!;
        public int CheckIntervalSeconds { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = default!;
        public ICollection<StatusPageService> StatusPageServices { get; set; } = new List<StatusPageService>();

    }

}
