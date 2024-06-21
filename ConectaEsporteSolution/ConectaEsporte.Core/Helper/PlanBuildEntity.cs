using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Helper
{
    public class PlanBuildEntity
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Finished { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

    }
}
