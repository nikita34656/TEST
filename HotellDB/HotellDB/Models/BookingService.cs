using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Models
{
    public class BookingService
    {
        public int booking_service_id { get; set; }
        public int booking_id { get; set; }
        public int service_id { get; set; }
        public int quantity { get; set; }
        public decimal price_at_booking { get; set; }
        public decimal subtotal { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual Service Service { get; set; }
    }
}
