using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Models
{
	public class RoomClassUser
	{
		public long Id { get; set; }
		public long UserId { get; set; }

		public long RoomClassId { get; set; }

		public bool Confirmed { get; set; }	
	}
}
