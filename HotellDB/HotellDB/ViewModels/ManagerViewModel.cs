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
    public class ManagerViewModel : BaseViewModel
    {
        private ObservableCollection<Booking> _allBookings;
        private ObservableCollection<Room> _allRooms;
        private Booking _selectedBooking;
        private string _searchText;
        private readonly RoomService _roomService;

        public ObservableCollection<Booking> AllBookings
        {
            get => _allBookings;
            set => SetProperty(ref _allBookings, value);
        }

        public ObservableCollection<Room> AllRooms
        {
            get => _allRooms;
            set => SetProperty(ref _allRooms, value);
        }

        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set => SetProperty(ref _selectedBooking, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterBookings();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ViewRoomsCommand { get; }

        public ManagerViewModel()
        {
            _roomService = new RoomService();
            RefreshCommand = new RelayCommand(LoadData);
            LogoutCommand = new RelayCommand(Logout);
            ViewRoomsCommand = new RelayCommand(ViewRooms);

            LoadData();
        }

        private void LoadData()
        {
            var bookings = _roomService.GetAllBookings();
            AllBookings = new ObservableCollection<Booking>(bookings);

            var rooms = _roomService.GetAvailableRooms(System.DateTime.Today, System.DateTime.Today.AddDays(1));
            AllRooms = new ObservableCollection<Room>(rooms);
        }

        private void FilterBookings()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadData();
                return;
            }

            var filtered = _roomService.GetAllBookings()
                .Where(b => b.User?.full_name?.Contains(SearchText) == true ||
                           b.Room?.room_number?.Contains(SearchText) == true)
                .ToList();

            AllBookings = new ObservableCollection<Booking>(filtered);
        }

        private void ViewRooms()
        {
            NavigationService.Navigate(new RoomsViewModel());
        }

        private void Logout()
        {
            CurrentUser.User = null;
            NavigationService.Navigate(new LoginViewModel());
        }
    }
}
