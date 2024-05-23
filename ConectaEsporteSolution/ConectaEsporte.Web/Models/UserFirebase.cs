namespace ConectaEsporte.Web.Models
{
	public class UserFirebase
	{
		public string uid { get; set; }
		public string email { get; set; }
		public string fcm_token { get; set; }
		public string name { get; set; }				
		public DateTime created_time { get; set; }
	}
}
