using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Models
{
    public class RoomType
    {
        public int room_type_id { get; set; }
        public string name { get; set; } // Базовый, Продвинутый, Президентский
        public string description { get; set; }
        public decimal base_price_per_night { get; set; }
        public int max_occupancy { get; set; }
        public int area_sqm { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Bookings> Bookings { get; set; }
    }
}
