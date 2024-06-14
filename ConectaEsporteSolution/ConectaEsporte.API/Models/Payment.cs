namespace ConectaEsporte.API.Models
{
    public class Payment
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }

        public long UserId { get; set; }

        public string UserEmail { get; set; }


        public PlanItem PlanSelected { get; set; } = new PlanItem();

        public List<PlanItem> Plans { get; set; } = new List<PlanItem>();

    }


    public class PaymentItem
    {
        public long Id { get; set; }

    }
    public class PlanItem
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string PriceFormat { get; set; }

        public string Type { get; set; }
        public int Order { get; set; }

        public DateTime Created { get; set; }

    }
}
