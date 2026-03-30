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
    public class MyBookingsViewModel : BaseViewModel
    {
        private ObservableCollection<Booking> _bookings;
        private Booking _selectedBooking;
        private readonly RoomService _roomService;

        public ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            set => SetProperty(ref _bookings, value);
        }

        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set => SetProperty(ref _selectedBooking, value);
        }

        public ICommand CancelBookingCommand { get; }
        public ICommand BackCommand { get; }

        public MyBookingsViewModel()
        {
            _roomService = new RoomService();
            CancelBookingCommand = new RelayCommand(CancelBooking, () => SelectedBooking != null);
            BackCommand = new RelayCommand(GoBack);

            LoadBookings();
        }

        private void LoadBookings()
        {
            var bookings = _roomService.GetUserBookings(CurrentUser.User.user_id);
            Bookings = new ObservableCollection<Booking>(bookings);
        }

        private void CancelBooking()
        {
            if (System.Windows.MessageBox.Show("Отменить бронирование?", "Подтверждение",
                System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
            {
                _roomService.CancelBooking(SelectedBooking.booking_id);
                LoadBookings();
            }
        }

        private void GoBack()
        {
            NavigationService.Navigate(new RoomsViewModel());
        }
    }
}
