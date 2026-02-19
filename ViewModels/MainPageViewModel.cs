using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitApp.App.Models;
using FitApp.App.Models.DTO;
using FitApp.App.Resources.Languages;
using FitApp.App.Services;
using FitApp.App.Views;
using FitApp.App.Views.Popups;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FitApp.App.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly DailyService _dailyService;

        public MainPageViewModel(
            UserService userService,
            DailyService dailyService)
        {
            _userService = userService;
            _dailyService = dailyService;

            LoadCommand = new AsyncRelayCommand(LoadAsync);
        }

        [ObservableProperty] private ObservableCollection<MealEntryDto> meals = new();

        [ObservableProperty] private int caloriesEaten;
        [ObservableProperty] private int caloriesLeft;

        [ObservableProperty] private double carbon;
        [ObservableProperty] private double protein;
        [ObservableProperty] private double fat;
        [ObservableProperty] private double waterConsumed;

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string userName = string.Empty;

        [ObservableProperty] private DateTime selectedDate = DateTime.UtcNow.Date;
        [ObservableProperty] private string actualDailyDate = DateTime.UtcNow.Date.ToString("dd.MM.yyyy");


        public IAsyncRelayCommand LoadCommand { get; }


        [ObservableProperty] private DailyReport? currentDaily;

        public async Task LoadAsync()
        {
            var today = DateTime.UtcNow.Date;
            SelectedDate = today;
            await LoadForSelectedDate(today);
        }


        [RelayCommand]
        private async Task ChangeWater()
        {
            if (IsBusy)
                return;

            var userId = Preferences.Get("UserId", 0);
            if (userId == 0)
                return;

            var date = SelectedDate.Date;

            var daily = await _dailyService.GetDailyByDateAsync(userId, date);
            if (daily == null)
                return;

            string? input = await Application.Current.MainPage.DisplayPromptAsync(
                AppResources.waterText,
                AppResources.changeWaterInfo,
                initialValue: "100",
                keyboard: Keyboard.Numeric);

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!double.TryParse(input, out double water) || water < 0)
                return;

            var newWaterTotal = (daily.Water ?? 0) + water;

            var success = await _dailyService.UpdateWaterAsync(daily.Id, newWaterTotal);
            if (!success)
                return;

            WaterConsumed = newWaterTotal;
        }

        [RelayCommand]
        private async Task OpenSettingsPage()
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        [RelayCommand]
        private async Task OpenMealsPage()
        {
            await Shell.Current.GoToAsync(
                $"{nameof(MealsPage)}?date={SelectedDate:yyyy-MM-dd}");
        }

        [RelayCommand]
        private async Task OpenCalendar()
        {
            var popup = new CalendarPopup();

            var result = await Application.Current.MainPage.ShowPopupAsync(popup);

            if (result is DateTime pickedDate)
            {
                SelectedDate = pickedDate;
                ActualDailyDate = pickedDate.ToString("dd.MM.yyyy");

                await LoadForSelectedDate(pickedDate);
            }
        }

        public async Task LoadForSelectedDate(DateTime date)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserName = Preferences.Get("UserName", string.Empty);

                var userId = Preferences.Get("UserId", 0);
                if (userId == 0)
                    return;

                SelectedDate = date.Date;
                ActualDailyDate = SelectedDate.ToString("dd.MM.yyyy");

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                    return;

                var daily = await _dailyService.GetDailyByDateAsync(userId, SelectedDate);

                if (daily == null)
                {
                    var createdId = await _dailyService.CreateDailyAsync(userId, SelectedDate);
                    if (createdId == null)
                        return;

                    daily = await _dailyService.GetDailyByDateAsync(userId, SelectedDate);
                    if (daily == null)
                        return;
                }

                CaloriesEaten = daily.Calories;
                Protein = daily.Protein ?? 0;
                Carbon = daily.Carbon ?? 0;
                Fat = daily.Fat ?? 0;

                CaloriesLeft = user.DailyCalorieGoal - CaloriesEaten;
                if (CaloriesLeft < 0)
                    CaloriesLeft = 0;

                WaterConsumed = daily.Water ?? 0;

                Meals.Clear();
                foreach (var meal in daily.Meals ?? new List<MealEntryDto>())
                {
                    Meals.Add(meal);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
