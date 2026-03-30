using HotellDB.Models;
using HotellDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace HotellDB.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _email;
        private string _phone;
        private string _fullName;
        private string _errorMessage;
        private readonly AuthService _authService;

        public string Username { get => _username; set => SetProperty(ref _username, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string ConfirmPassword { get => _confirmPassword; set => SetProperty(ref _confirmPassword, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }
        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }

        public RegisterViewModel()
        {
            _authService = new AuthService();
            RegisterCommand = new RelayCommand(Register);
            BackCommand = new RelayCommand(GoBack);
        }

        private void Register()
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Пароли не совпадают";
                return;
            }

            var user = new User
            {
                username = Username,
                email = Email,
                phone = Phone,
                full_name = FullName
            };

            if (_authService.Register(user, Password))
            {
                NavigationService.Navigate(new LoginViewModel());
            }
            else
            {
                ErrorMessage = "Пользователь с таким именем или email уже существует";
            }
        }

        private void GoBack()
        {
            NavigationService.Navigate(new LoginViewModel());
        }
    }
}
