using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Models
{
    public class Payment
    {
        public int payment_id { get; set; }
        public int booking_id { get; set; }
        public decimal amount { get; set; }
        public DateTime payment_date { get; set; }
        public string payment_method { get; set; }
        public string status { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
