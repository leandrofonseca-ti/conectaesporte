using ConectaEsporte.Core.Helper;
using ConectaEsporte.Core.Models;

namespace ConectaEsporte.API.Models
{
    public class PaymentModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string UserEmail { get; set; }

        public PlanUserEntity PlanSelected { get; set; } = new PlanUserEntity();

        public List<PlanEntity> Plans { get; set; } = new List<PlanEntity>();

        public bool Active {  get; set; }

        public bool WillExpire { get; set; }
    }


    public class PaymentBuildModel
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public string CreatedFormat { get { return Created.ToString("dd/MM/yyyy"); } }
        public string CreatedTimeFormat { get { return Created.ToString("HH:mm"); } }
        public DateTime Finished { get; set; }
        public string FinishedFormat { get { return Finished.ToString("dd/MM/yyyy"); } }
        public string FinishedTimeFormat { get { return Finished.ToString("HH:mm"); } }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }



    public class PaymentItem
    {
        public long Id { get; set; }

    }
 
}
