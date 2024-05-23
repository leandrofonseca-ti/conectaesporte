namespace ConectaSolution.Models
{
	public class VMLogin
	{
		public string Name { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public bool KeepLogin { get; set; } = true;
	}
}
