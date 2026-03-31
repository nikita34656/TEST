using HotellDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Services
{
    public class AuthService
    {
        private readonly HotelDbContext _context;

        public AuthService()
        {
            _context = new HotelDbContext();
        }

        public User Login(string username, string password)
        {
            var hash = HashPassword(password);
            var user = _context.Users.FirstOrDefault(u => u.username == username && u.password_hash == hash);

            if (user != null)
            {
                user.last_login = System.DateTime.Now;
                _context.SaveChanges();
            }

            return user;
        }

        public bool Register(User user, string password)
        {
            if (_context.Users.Any(u => u.username == user.username || u.email == user.email))
                return false;

            user.password_hash = HashPassword(password);
            user.created_at = System.DateTime.Now;
            user.role = "Клиент";

            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(bytes);
        }
    }
}
