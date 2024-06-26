using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Models
{
	public class RoomClass
	{
		public long Id { get; set; }
		public long OwnerId { get; set; }
        public long LocalId { get; set; }
        public string Title { get; set; }

		public string Type { get; set; }
		public string SubTitle { get; set; }

        public string Picture { get; set; }
        public bool Active { get; set; }
	}
}
