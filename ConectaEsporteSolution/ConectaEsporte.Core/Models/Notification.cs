using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Models
{
	public class Notification
	{
		public long Id { get; set; }
		public string Title { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string FromPicture { get; set; }

        public bool IsRead { get; set; }

        public long CheckinId { get; set; }

        public DateTime Created { get; set; }
    }
}
