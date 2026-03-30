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
    public class RoomsViewModel : BaseViewModel
    {
        private DateTime _checkInDate;
        private DateTime _checkOutDate;
        private RoomType _selectedRoomType;
        private ObservableCollection<Room> _availableRooms;
        private ObservableCollection<RoomType> _roomTypes;
        private Room _selectedRoom;
        private bool _includeMeal;
        private bool _includeInternet;
        private readonly RoomService _roomService;

        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                SetProperty(ref _checkInDate, value);
                LoadAvailableRooms();
            }
        }

        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                SetProperty(ref _checkOutDate, value);
                LoadAvailableRooms();
            }
        }

        public RoomType SelectedRoomType
        {
            get => _selectedRoomType;
            set
            {
                SetProperty(ref _selectedRoomType, value);
                LoadAvailableRooms();
            }
        }

        public ObservableCollection<Room> AvailableRooms
        {
            get => _availableRooms;
            set => SetProperty(ref _availableRooms, value);
        }

        public ObservableCollection<RoomType> RoomTypes
        {
            get => _roomTypes;
            set => SetProperty(ref _roomTypes, value);
        }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set => SetProperty(ref _selectedRoom, value);
        }

        public bool IncludeMeal
        {
            get => _includeMeal;
            set => SetProperty(ref _includeMeal, value);
        }

        public bool IncludeInternet
        {
            get => _includeInternet;
            set => SetProperty(ref _includeInternet, value);
        }

        public ICommand BookRoomCommand { get; }
        public ICommand MyBookingsCommand { get; }
        public ICommand LogoutCommand { get; }

        public RoomsViewModel()
        {
            _roomService = new RoomService();
            _checkInDate = DateTime.Today;
            _checkOutDate = DateTime.Today.AddDays(1);

            BookRoomCommand = new RelayCommand(BookRoom, () => SelectedRoom != null);
            MyBookingsCommand = new RelayCommand(ShowMyBookings);
            LogoutCommand = new RelayCommand(Logout);

            LoadRoomTypes();
            LoadAvailableRooms();
        }

        private void LoadRoomTypes()
        {
            var types = _roomService.GetAllRoomTypes();
            RoomTypes = new ObservableCollection<RoomType>(types);
            RoomTypes.Insert(0, new RoomType { room_type_id = 0, name = "Все типы" });
        }

        private void LoadAvailableRooms()
        {
            if (CheckInDate >= CheckOutDate)
                return;

            var roomTypeId = SelectedRoomType?.room_type_id > 0 ? SelectedRoomType.room_type_id : (int?)null;
            var rooms = _roomService.GetAvailableRooms(CheckInDate, CheckOutDate, roomTypeId);
            AvailableRooms = new ObservableCollection<Room>(rooms);
        }

        private void BookRoom()
        {
            var bookingVM = new BookingViewModel(SelectedRoom, CheckInDate, CheckOutDate, IncludeMeal, IncludeInternet);
            NavigationService.Navigate(bookingVM);
        }

        private void ShowMyBookings()
        {
            NavigationService.Navigate(new MyBookingsViewModel());
        }

        private void Logout()
        {
            CurrentUser.User = null;
            NavigationService.Navigate(new LoginViewModel());
        }
    }
}
