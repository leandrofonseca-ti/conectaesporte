namespace ConectaEsporte.API.Models
{
    public class NotificationModel
    {
        public long Id { get; set; }
        public string SenderEmail { get; set; }
        public string SenderImage { get; set; }

        public string UserEmail { get; set; }
        public DateTime Created { get; set; }
        public string CreatedFormat { get { return Created.ToString("dd/MM/yyyy"); } }
        public string TimeFormat { get { return Created.ToString("HH:mm"); } }


        public bool IsRead { get; set; }
        public long ContentId { get; set; }
        public string Text { get; set; }
    }
}
