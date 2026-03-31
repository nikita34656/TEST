using HotellDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HotellDB.Services
{
    public class RoomService
    {
        private readonly HotelDbContext _context;

        public RoomService()
        {
            _context = new HotelDbContext();
        }

        public List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut, int? roomTypeId = null)
        {
            var bookedRoomIds = _context.Bookings
                .Where(b => b.status == "Активно" &&
                       ((checkIn >= b.check_in_date && checkIn < b.check_out_date) ||
                        (checkOut > b.check_in_date && checkOut <= b.check_out_date) ||
                        (checkIn <= b.check_in_date && checkOut >= b.check_out_date)))
                .Select(b => b.room_id)
                .Distinct()
                .ToList();

            var query = _context.Rooms
                .Include(r => r.RoomType)
                .Where(r => !bookedRoomIds.Contains(r.room_id) && r.status == "Свободен");

            if (roomTypeId.HasValue)
            {
                query = query.Where(r => r.room_type_id == roomTypeId.Value);
            }

            return query.ToList();
        }

        public List<RoomType> GetAllRoomTypes()
        {
            return _context.RoomTypes.ToList();
        }

        public List<Booking> GetUserBookings(int userId)
        {
            return _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.RoomType)
                .Include(b => b.BookingServices)
                .ThenInclude(bs => bs.Service)
                .Where(b => b.user_id == userId)
                .OrderByDescending(b => b.created_at)
                .ToList();
        }

        public List<Booking> GetAllBookings()
        {
            return _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Include(b => b.RoomType)
                .Include(b => b.BookingServices)
                .ThenInclude(bs => bs.Service)
                .OrderByDescending(b => b.created_at)
                .ToList();
        }

        public List<Service> GetActiveServices()
        {
            return _context.Services.Where(s => s.is_active).ToList();
        }

        public Booking CreateBooking(Booking booking, List<(Service service, int quantity)> services)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                foreach (var (service, quantity) in services)
                {
                    var bookingService = new BookingService
                    {
                        booking_id = booking.booking_id,
                        service_id = service.service_id,
                        quantity = quantity,
                        price_at_booking = service.price,
                        subtotal = service.price * quantity
                    };
                    _context.BookingServices.Add(bookingService);
                }

                _context.SaveChanges();
                transaction.Commit();
                return booking;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void UpdateBooking(Booking booking)
        {
            booking.updated_at = DateTime.Now;
            _context.Entry(booking).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void CancelBooking(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking != null)
            {
                booking.status = "Отменено";
                booking.updated_at = DateTime.Now;
                _context.SaveChanges();
            }
        }
    }
}
