using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitApp.App.Helpers;
using FitApp.App.Models;
using FitApp.App.Services;
using Microsoft.Maui.Storage;
using FitApp.App.Resources.Languages;

namespace FitApp.App.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly UserService _userService;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        public LoginViewModel(UserService userService)
        {
            _userService = userService;
            Title = AppResources.loginPageTitle;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert(AppResources.errorTitle, AppResources.emptyFieldsError, "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var user = await _userService.LoginAsync(Email, Password);

                if (user == null)
                {
                    await Shell.Current.DisplayAlert(AppResources.errorTitle, AppResources.invalidCredentialsError, "OK");
                    return;
                }

                Preferences.Set("UserId", user.UserId);
                Preferences.Set("UserName", user.UserName);
                Preferences.Set("UserEmail", user.UserEmail);
                Preferences.Set("IsLoggedIn", true);

                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert(AppResources.errorTitle, AppResources.loginExceptionError, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CreateAccountAsync()
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }

    }
}
