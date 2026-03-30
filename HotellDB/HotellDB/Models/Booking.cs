using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Models
{
    public class Booking
    {
        public int booking_id { get; set; }
        public int user_id { get; set; }
        public int room_id { get; set; }
        public int room_type_id { get; set; }
        public DateTime check_in_date { get; set; }
        public DateTime check_out_date { get; set; }
        public int num_guests { get; set; }
        public string status { get; set; } // Активно, Отменено, Завершено
        public decimal total_price { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int? created_by { get; set; }
        public string notes { get; set; }

        public virtual User User { get; set; }
        public virtual Room Room { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual ICollection<Booking_Services> BookingServices { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }
    }
}
