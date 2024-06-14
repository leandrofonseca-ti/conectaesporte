using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string Picture { get; set; }

		public string Phone { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

        public string Fcm { get; set; }
        public string KeyMobile { get; set; }

        public DateTime Created_Date { get; set; }
		//public List<Profile> Profiles { get; set; } = new List<Profile>();
	}

	public class UserEntity : User
	{
		public List<Profile> Profiles { get; set; } = new List<Profile>();
	}
}
