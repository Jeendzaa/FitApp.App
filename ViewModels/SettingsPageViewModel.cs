using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitApp.App.Services;

namespace FitApp.App.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        private readonly UserService _userService;

        public SettingsPageViewModel(UserService userService)
        {
            _userService = userService;
            _ = LoadAsync();
        }

        [ObservableProperty] private string userName = string.Empty;

        [ObservableProperty] private int calories;

        [ObservableProperty] private int userWeight;
        [ObservableProperty] private bool isBusy;

        public async Task LoadAsync()
        {
            var userId = Preferences.Get("UserId", 0);
            if (userId == 0)
                return;

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return;

            UserName = user.UserName;
            Calories = user.DailyCalorieGoal;
            UserWeight = user.UserCurrentWeight;
        }

        [RelayCommand]
        private async Task ChangeWeight()
        {
            if (IsBusy)
                return;

            var userId = Preferences.Get("UserId", 0);
            if (userId == 0)
                return;

            string? input = await Application.Current.MainPage.DisplayPromptAsync(
                "Weight",
                "Podaj nową wagę (kg):",
                initialValue: UserWeight.ToString(),
                keyboard: Keyboard.Numeric);

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!int.TryParse(input, out int newWeight) || newWeight <= 0)
                return;

            var success = await _userService.UpdateWeightAsync(userId, newWeight);
            if (!success)
                return;

            UserWeight = newWeight;
        }


        [RelayCommand]
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
