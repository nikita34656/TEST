using HotellDB.Models;
using HotellDB.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace HotellDB.ViewModels
{
    public class BookingViewModel : BaseViewModel
    {
        private readonly Room _room;
        private readonly DateTime _checkInDate;
        private readonly DateTime _checkOutDate;
        private readonly RoomService _roomService;
        private ObservableCollection<Service> _services;
        private int _numGuests;
        private decimal _totalPrice;
        private string _notes;

        public Room Room => _room;
        public DateTime CheckInDate => _checkInDate;
        public DateTime CheckOutDate => _checkOutDate;
        public int Nights => (int)(_checkOutDate - _checkInDate).TotalDays;

        public ObservableCollection<Service> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public int NumGuests
        {
            get => _numGuests;
            set
            {
                SetProperty(ref _numGuests, value);
                CalculateTotalPrice();
            }
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public ICommand ConfirmBookingCommand { get; }
        public ICommand BackCommand { get; }

        public BookingViewModel(Room room, DateTime checkIn, DateTime checkOut, bool includeMeal, bool includeInternet)
        {
            _room = room;
            _checkInDate = checkIn;
            _checkOutDate = checkOut;
            _roomService = new RoomService();
            _numGuests = 1;

            ConfirmBookingCommand = new RelayCommand(ConfirmBooking);
            BackCommand = new RelayCommand(GoBack);

            LoadServices(includeMeal, includeInternet);
            CalculateTotalPrice();
        }

        private void LoadServices(bool includeMeal, bool includeInternet)
        {
            var allServices = _roomService.GetActiveServices();
            Services = new ObservableCollection<Service>();

            foreach (var service in allServices)
            {
                if ((service.name == "Питание" && includeMeal) ||
                    (service.name == "Интернет" && includeInternet) ||
                    (service.name != "Питание" && service.name != "Интернет"))
                {
                    Services.Add(service);
                }
            }
        }

        private void CalculateTotalPrice()
        {
            decimal roomPrice = _room.RoomType.base_price_per_night * Nights;
            decimal servicesPrice = Services.Sum(s => s.price);
            TotalPrice = roomPrice + servicesPrice;
        }

        private void ConfirmBooking()
        {
            var booking = new Booking
            {
                user_id = CurrentUser.User.user_id,
                room_id = _room.room_id,
                room_type_id = _room.room_type_id,
                check_in_date = _checkInDate,
                check_out_date = _checkOutDate,
                num_guests = _numGuests,
                status = "Активно",
                total_price = TotalPrice,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                created_by = CurrentUser.User.user_id,
                notes = _notes
            };

            var servicesWithQuantity = Services.Select(s => (s, 1)).ToList();
            _roomService.CreateBooking(booking, servicesWithQuantity);

            System.Windows.MessageBox.Show("Бронирование успешно создано!", "Успех",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            NavigationService.Navigate(new RoomsViewModel());
        }

        private void GoBack()
        {
            NavigationService.Navigate(new RoomsViewModel());
        }
    }
}
