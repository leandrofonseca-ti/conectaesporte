using System.ComponentModel.DataAnnotations;

namespace ConectaEsporte.API.Models
{
    [Serializable]
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
