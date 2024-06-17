using ConectaEsporte.Core.Helper;
using ConectaEsporte.Core.Models;

namespace ConectaEsporte.API.Models
{
    public class PaymentModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string UserEmail { get; set; }

        public PlanUser PlanSelected { get; set; } = new PlanUser();

        public List<PlanEntity> Plans { get; set; } = new List<PlanEntity>();

        public bool Active {  get; set; }

        public bool WillExpire { get; set; }
    }


    public class PaymentItem
    {
        public long Id { get; set; }

    }
 
}
