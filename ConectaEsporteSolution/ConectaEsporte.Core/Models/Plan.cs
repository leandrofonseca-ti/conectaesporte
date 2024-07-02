using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Models
{
	public class Plan
	{
		public long Id { get; set; }

        public long GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int TimesOfWeek { get; set; }
        private decimal _price { get; set; }
        public decimal Price { get { return decimal.Round(_price, 2, MidpointRounding.AwayFromZero); } set { _price = value; } }
        public string PriceFmt { get { return string.Format("{0:0,0.00}", _price); } }
        public int  Order { get; set; }
        public bool Active { get; set; }

        public int PeriodMonth { get; set; }
    }
}
