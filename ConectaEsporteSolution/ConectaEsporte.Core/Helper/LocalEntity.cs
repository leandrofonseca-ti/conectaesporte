using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Helper
{

    [Serializable]
    public class LocalEntity
    {
        public long Id { get; set; }

        public long OwnerId { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
    }
}
