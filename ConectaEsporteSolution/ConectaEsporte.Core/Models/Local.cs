using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Models
{
	public class Local
	{
		public long Id { get; set; }

        public long OwnerId { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
    }
}
