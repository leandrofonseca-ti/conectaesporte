using System;
using System.Collections.Generic;
using System.Text;
using Uol.PagSeguro.NET8.Service;

namespace Uol.PagSeguro.NET8.Domain.Direct
{
    public class BoletoCheckout : Checkout
    {
        /// <summary>
        /// Payment Method
        /// </summary>
        public string PaymentMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BoletoCheckout()
        {
            this.PaymentMethod = "Boleto";
        }
    }
}
