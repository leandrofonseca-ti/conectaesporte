using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Models
{
    public class PlanUserHistory
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
        public long OwnerId { get; set; }

    }
}
