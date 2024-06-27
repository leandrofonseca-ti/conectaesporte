using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Uol.Models
{
    public class PaymentItemEntity
    {
        public string Code { get;  set; }
        public string Description { get;  set; }
        public decimal Price { get;  set; }
        public string UserReference { get;  set; }
        public string UserName { get;  set; }
        public string UserEmail { get;  set; }
        public string UserPhoneDDD { get;  set; }
        public string UserPhone { get;  set; }
    }
}
