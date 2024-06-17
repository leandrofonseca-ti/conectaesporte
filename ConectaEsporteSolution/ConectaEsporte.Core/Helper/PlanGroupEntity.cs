using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Helper
{
 

    public class PlanEntity
    {
        public long Id { get; set; }

        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string ProfileIds { get; set; }
        public int GroupOrder { get; set; }
        public long GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Order { get; set; }
        public bool Active { get; set; }
    }
}
