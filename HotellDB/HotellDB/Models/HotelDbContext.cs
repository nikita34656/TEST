using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace HotellDB.Models
{
    internal class HotelDbContext
    {
        public class HotelDbContext : DbContext
        {
            public DbSet<Users> Users { get; set; }
            public DbSet<Rooms> Rooms { get; set; }
            public DbSet<Room_Types> RoomTypes { get; set; }
            public DbSet<Bookings> Bookings { get; set; }
            public DbSet<Services> Services { get; set; }
            public DbSet<Booking_Services> BookingServices { get; set; }
            public DbSet<Payments> Payments { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost;Database=HotelRegistry;Trusted_Connection=True;TrustServerCertificate=True;");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Bookings>()
                    .Property(b => b.total_price)
                    .HasPrecision(12, 2);

                modelBuilder.Entity<Booking_Services>()
                    .Property(bs => bs.price_at_booking)
                    .HasPrecision(10, 2);

                modelBuilder.Entity<Booking_Services>()
                    .Property(bs => bs.subtotal)
                    .HasPrecision(12, 2);

                modelBuilder.Entity<Payments>()
                    .Property(p => p.amount)
                    .HasPrecision(12, 2);

                modelBuilder.Entity<Room_Types>()
                    .Property(rt => rt.base_price_per_night)
                    .HasPrecision(10, 2);

                modelBuilder.Entity<Services>()
                    .Property(s => s.price)
                    .HasPrecision(10, 2);
            }
        }
    }
}
