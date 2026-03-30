using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Models
{
    public class User
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string password_hash { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string full_name { get; set; }
        public string role { get; set; } // Гость, Клиент, Менеджер
        public DateTime created_at { get; set; }
        public DateTime? last_login { get; set; }

        public virtual ICollection<Bookings> Bookings { get; set; }
    }
}
