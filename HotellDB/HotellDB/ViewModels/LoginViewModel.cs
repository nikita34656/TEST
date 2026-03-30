using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using HotellDB.Models;
using HotellDB.Services;

namespace HotellDB.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private readonly AuthService _authService;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(OpenRegister);
        }

        private void Login()
        {
            var user = _authService.Login(Username, Password);
            if (user != null)
            {
                CurrentUser.User = user;

                if (user.role == "Менеджер")
                    NavigationService.Navigate(new ManagerViewModel());
                else
                    NavigationService.Navigate(new RoomsViewModel());
            }
            else
            {
                ErrorMessage = "Неверное имя пользователя или пароль";
            }
        }

        private void OpenRegister()
        {
            NavigationService.Navigate(new RegisterViewModel());
        }
    }
}
