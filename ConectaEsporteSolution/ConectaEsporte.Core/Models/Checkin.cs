using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Models
{
	public class Checkin
	{
		public long Id { get; set; }
		public string Title { get; set; }
        public string Email { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public DateTime BookedDt { get; set; }

        public DateTime? ConfirmDt { get; set; }
        public bool Booked { get; set; }
    }
}
