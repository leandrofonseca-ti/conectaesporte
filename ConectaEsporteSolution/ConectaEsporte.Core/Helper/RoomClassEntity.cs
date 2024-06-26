using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConectaEsporte.Core.Helper
{
    [Serializable]
    public class RoomClassEntity
    {
        public RoomClassEntity() { }
        public long Id { get; set; }

        public long LocalId { get; set; }

        public long OwnerId { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public LocalEntity Local { get; set; } = new LocalEntity();

        public List<UserViewEntity> People { get; set; } = new List<UserViewEntity>();
        
    }


    [Serializable]
    public class RoomClassDetailEntity
    {
        public RoomClassDetailEntity() { }
        public long Id { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }

    }
}
