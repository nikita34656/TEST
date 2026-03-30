using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Models
{
    public class Room
    {
        public int room_id { get; set; }
        public string room_number { get; set; }
        public int room_type_id { get; set; }
        public int floor { get; set; }
        public string status { get; set; } // Свободен, Занят, На ремонте
        public string description { get; set; }

        public virtual Room_Types RoomType { get; set; }
        public virtual ICollection<Bookings> Bookings { get; set; }
    }
}
