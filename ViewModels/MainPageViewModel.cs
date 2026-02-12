using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitApp.App.Models;
using FitApp.App.Models.DTO;
using FitApp.App.Services;

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


        [ObservableProperty] private int caloriesEaten;
        [ObservableProperty] private int caloriesLeft;

        [ObservableProperty] private double carbon;
        [ObservableProperty] private double protein;
        [ObservableProperty] private double fat;

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string userName = string.Empty;

        public IAsyncRelayCommand LoadCommand { get; }


        [ObservableProperty] private DailyReport? currentDaily;

        public async Task LoadAsync()
        {
            IsBusy = true;

            try
            {
                UserName = Preferences.Get("UserName", string.Empty);

                var userId = Preferences.Get("UserId", 0);
                if (userId == 0)
                    return;

                var today = DateTime.UtcNow.Date;

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                    return;

                // 2️⃣ DAILY
                var daily = await _dailyService.GetDailyByDateAsync(userId, today);

                if (daily == null)
                {
                    var createdId = await _dailyService.CreateDailyAsync(userId, today);
                    if (createdId == null)
                        return;

                    daily = await _dailyService.GetDailyByDateAsync(userId, today);
                    if (daily == null)
                        return;
                }

                CurrentDaily = daily;

                CaloriesEaten = daily.DailyReportCalories;
                CaloriesLeft = user.DailyCalorieGoal - CaloriesEaten;

                if (CaloriesLeft < 0)
                    CaloriesLeft = 0;
            }
            finally
            {
                IsBusy = false;
            }
        }




    }
}
