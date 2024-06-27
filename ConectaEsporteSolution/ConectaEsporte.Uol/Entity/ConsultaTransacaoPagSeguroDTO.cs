using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Uol.Entity
{
    public class ConsultaTransacaoPagSeguroDTO
    {
        public DateTime Date { get; set; }
        public int ResultsInThisPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public List<ConsultaTransacaoPagSeguroTransactionDTO> listTransaction { get; set; }
    }
}
