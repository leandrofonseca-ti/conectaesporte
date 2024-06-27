using ConectaEsporte.Uol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Uol.Repository
{
    public interface IPaymentRepository
    {
        PaymentResponseEntity CreatePayment(PaymentItemEntity entity, AppSetupEntity setup);
    }
}
