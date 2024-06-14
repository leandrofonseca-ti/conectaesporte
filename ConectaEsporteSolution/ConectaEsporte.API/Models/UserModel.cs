using System.ComponentModel.DataAnnotations;

namespace ConectaEsporte.API.Models
{
    [Serializable]
    public class UserModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Key { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Fcm { get; set; } = string.Empty;

        public string Picture { get; set; } = string.Empty;
        

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
