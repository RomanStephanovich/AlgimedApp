
using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.Dtos;
using AlgimedApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AlgimedApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username = "";
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        private readonly IAuthService _authService;

        public event Action? LoginSucceeded;

        public LoginViewModel()
        {
            _authService = App.AppHost.Services.GetRequiredService<IAuthService>();
            LoginCommand = new RelayCommand(async _ => await LoginAsync());
            RegisterCommand = new RelayCommand(async _ => await RegisterAsync());
        }

        private async Task LoginAsync()
        {
            var result = await _authService.AuthenticateAsync(new LoginDto
            {
                Username = Username,
                PasswordHash = Password
            });

            MessageBox.Show(result);

            if (result == "Login successful")
            {
                LoginSucceeded?.Invoke(); 
            }
        }

        private async Task RegisterAsync()
        {
            var result = await _authService.RegisterAsync(new LoginDto
            {
                Username = Username,
                PasswordHash = Password
            });

            MessageBox.Show(result);
        }
    }
}