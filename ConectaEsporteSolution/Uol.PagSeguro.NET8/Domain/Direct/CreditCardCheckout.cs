﻿using System;
using System.Collections.Generic;
using System.Text;
using Uol.PagSeguro.NET8.Service;

namespace Uol.PagSeguro.NET8.Domain.Direct
{
    public class CreditCardCheckout : Checkout
    {

        /// <summary>
        /// Credit Card Token
        /// </summary>
        public String Token
        {
            get;
            set;
        }

        /// <summary>
        /// Installment
        /// </summary>
        public Installment Installment
        {
            get;
            set;
        }

        /// <summary>
        /// Holder 
        /// </summary>
        public Holder Holder
        {
            get;
            set;
        }

        /// <summary>
        /// Billing
        /// </summary>
        public Billing Billing
        {
            get;
            set;
        }

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
        public CreditCardCheckout()
        {
            this.PaymentMethod = "CreditCard";
        }
    }
}
