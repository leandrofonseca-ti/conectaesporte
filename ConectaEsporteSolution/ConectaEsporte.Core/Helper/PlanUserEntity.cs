using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Helper
{
    public class PlanUserEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PlanId { get; set; }

        public bool Free { get; set; }        
        public string GroupName { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Finished { get; set; }

        public string CreatedFormat { get { return Created.ToString("dd/MM/yyyy"); } }
        public string CreatedTimeFormat { get { return Created.ToString("HH:mm"); } }

        public string FinishedFormat { get { return Finished.ToString("dd/MM/yyyy"); } }
        public string FinishedTimeFormat { get { return Finished.ToString("HH:mm"); } }

    }
}
