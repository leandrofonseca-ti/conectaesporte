using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Helper
{
    public class PlanUserEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public decimal _amount { get; set; }
        public decimal Amount { get { return decimal.Round(_amount, 2, MidpointRounding.AwayFromZero); } set { _amount = value; } }
        public string AmountFmt { get { return string.Format("{0:0,0.00}", Amount); } }

        public DateTime Created { get; set; }
        public DateTime Finished { get; set; }

        public string CreatedFormat { get { return Created.ToString("dd/MM/yyyy"); } }
        public string CreatedTimeFormat { get { return Created.ToString("HH:mm"); } }

        public string FinishedFormat { get { return Finished.ToString("dd/MM/yyyy"); } }
        public string FinishedTimeFormat { get { return Finished.ToString("HH:mm"); } }

    }
}
