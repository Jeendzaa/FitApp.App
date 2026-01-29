using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitApp.App.Models;
using FitApp.App.Services;
using System.Text.RegularExpressions;

namespace FitApp.App.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty] private string login;
        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private DateTime dateOfBirth = DateTime.Today;
        [ObservableProperty] private string weight;
        [ObservableProperty] private string height;

        private readonly UserService _userService;

        public RegisterViewModel(UserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Weight) ||
                string.IsNullOrWhiteSpace(Height))
            {
                await Shell.Current.DisplayAlert("Error", "All fields must be filled.", "OK");
                return;
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(Email))
            {
                await Shell.Current.DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            if (Password.Length < 8 || Password.Length > 30)
            {
                await Shell.Current.DisplayAlert("Error", "Password must be 8–30 characters long.", "OK");
                return;
            }

            if (!int.TryParse(Weight, out int parsedWeight) || parsedWeight <= 0)
            {
                await Shell.Current.DisplayAlert("Error", "Weight must be a positive number.", "OK");
                return;
            }

            if (!int.TryParse(Height, out int parsedHeight) || parsedHeight <= 0)
            {
                await Shell.Current.DisplayAlert("Error", "Height must be a positive number.", "OK");
                return;
            }

            double heightMeters = parsedHeight / 100.0;
            int bmi = (int)(parsedWeight / (heightMeters * heightMeters));

            var newUser = new User
            {
                UserName = Login,
                UserEmail = Email,
                UserPassword = Password,
                UserDateOfBirth = DateOfBirth,
                UserCurrentWeight = parsedWeight,
                UserBmi = bmi
            };

            var success = await _userService.RegisterAsync(newUser);

            if (!success)
            {
                await Shell.Current.DisplayAlert("Error", "Email is already taken.", "OK");
                return;
            }

            await Shell.Current.DisplayAlert("Success", "Account created!", "OK");
            await Shell.Current.GoToAsync("//LoginPage");
        }

        [RelayCommand]
        private async Task BackToLoginPageAsync()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

    }
}
