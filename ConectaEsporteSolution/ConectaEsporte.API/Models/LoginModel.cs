using System.ComponentModel.DataAnnotations;

namespace ConectaEsporte.API.Models
{
    [Serializable]
    public class LoginModel
    {

        [Required]
        public string Email { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Fcm { get; set; }
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
    }


    [Serializable]
    public class LoginMailModel
    {
        [Required]
        public string Email { get; set; }

    }

    [Serializable]
    public class LoginPaymentSetModel
    {
        [Required]
        public string Email { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public long OwnerId { get; set; }
    }


    [Serializable]
    public class LoginPaymentModel
    {
        [Required]
        public string Email { get; set; }
        public long PlanId { get; set; }
    }


    [Serializable]
    public class LoginCheckinModel
    {
        [Required]
        public string Email { get; set; }

        public long Id { get; set; }

        public bool Booked { get; set; }

    }


    [Serializable]
    public class ListEventModel
    {
        [Required]
        public string Email { get; set; }

        public int PageIndex { get; set; }

        public string Type { get; set; }
    }

    [Serializable]
    public class ListEventDetailModel
    {
        [Required]
        public string Email { get; set; }

        public long Id { get; set; }

    }

}
