﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Models
{
	public class PlanUser
    {
		public long Id { get; set; }
        public long UserId { get; set; }
        public long PlanId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Finished { get; set; }

    }
}
