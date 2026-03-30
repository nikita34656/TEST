using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Models
{
    public class Service
    {
        public int service_id { get; set; }
        public string name { get; set; } // Питание, Интернет
        public string description { get; set; }
        public decimal price { get; set; }
        public string unit { get; set; }
        public bool is_active { get; set; }

        public virtual ICollection<Booking_Services> BookingServices { get; set; }
    }
}
