namespace ConectaEsporte.API.Models
{
    [Serializable]
    public class NotificationModel
    {
        public long Id { get; set; }
        public string SenderEmail { get; set; }
        public string SenderImage { get; set; }

        public string SenderName { get; set; }

        public string Email { get; set; }
        public DateTime Created { get; set; }
        public string CreatedFormat { get { return Created.ToString("dd/MM/yyyy"); } }
        public string TimeFormat { get { return Created.ToString("HH:mm"); } }


        public bool IsRead { get; set; }
        public long CheckinId { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
    }


    [Serializable]
    public class ParamIdentity
    {
        public long Id { get; set; }
    }
}
