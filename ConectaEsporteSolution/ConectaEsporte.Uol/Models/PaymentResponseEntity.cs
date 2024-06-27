using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uol.PagSeguro.NET8.Domain;

namespace ConectaEsporte.Uol.Models
{
    public class PaymentResponseEntity
    {
        public List<ServiceError> Errors { get; set; } = new List<ServiceError>();
        public string MessageError { get; set; }
        public bool Success { get; set; }
        public Uri Uri { get; set; }
    }
}
