using System.ComponentModel.DataAnnotations;

namespace ConectaEsporte.API.Models
{
    [Serializable]
    public class AuthModel
    {
        [Required]
        public string Key { get; set; }

    }
}
