using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitApp.App.Models.DTO;
using FitApp.App.Resources.Languages;
using FitApp.App.Services;
using System.Collections.ObjectModel;

namespace FitApp.App.ViewModels
{
    public partial class MealsPageViewModel : ObservableObject
    {
        private readonly MealService _mealService;
        private readonly MealEntryService _mealEntryService;
        private readonly DailyService _dailyService;

        [ObservableProperty]
        private string searchText = string.Empty;

        public ObservableCollection<MealDto> Meals { get; } = new();

        public MealsPageViewModel(
            MealService mealService,
            MealEntryService mealEntryService,
            DailyService dailyService)
        {
            _mealService = mealService;
            _mealEntryService = mealEntryService;
            _dailyService = dailyService;
        }

        [RelayCommand]
        public async Task SearchMealsAsync()
        {
            var result = await _mealService.SearchMealsAsync(SearchText);

            Meals.Clear();
            foreach (var meal in result)
                Meals.Add(meal);
        }

        [RelayCommand]
        public async Task AddMealAsync(MealDto meal)
        {
            if (meal == null)
                return;

            var userId = Preferences.Get("UserId", 0);
            if (userId == 0)
                return;

            var today = DateTime.UtcNow.Date;

            var daily = await _dailyService.GetDailyByDateAsync(userId, today);

            if (daily == null)
            {
                var createdId = await _dailyService.CreateDailyAsync(userId, today);
                if (createdId == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Cannot create daily report", "OK");
                    return;
                }

                daily = await _dailyService.GetDailyByDateAsync(userId, today);
                if (daily == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Daily still null after creation", "OK");
                    return;
                }
            }

            string? qtyStr = await Application.Current.MainPage.DisplayPromptAsync(
                AppResources.mealPageQuantity,
                $"{AppResources.mealPageHowManyText} {meal.MealName}?",
                initialValue: "1",
                keyboard: Keyboard.Numeric);

            if (!int.TryParse(qtyStr, out int quantity) || quantity <= 0)
                return;

            var success = await _mealEntryService.AddMealEntryAsync(daily.Id, meal.MealId, quantity);

            if (!success)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to add meal entry", "OK");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(AppResources.mealPageAddMealSucces, AppResources.mealPageAddMealSuccesAdded, "OK");
        }




    }
}
