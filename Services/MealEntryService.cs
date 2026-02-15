using System.Net.Http.Json;

namespace FitApp.App.Services
{
    public class MealEntryService
    {
        private readonly HttpClient _http;

        public MealEntryService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://fitappapi.azurewebsites.net/api/");
        }

        public async Task<bool> AddMealEntryAsync(int dailyId, int mealId, int quantity)
        {
            var body = new
            {
                MealId = mealId,
                DailyReportId = dailyId,
                MealEntryQuantity = quantity,
                MealEntryDate = DateTime.UtcNow
            };

            var response = await _http.PostAsJsonAsync("MealEntry", body);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine("ADD MEAL ENTRY ERROR: " + error);
                return false;
            }

            return true;
        }

    }
}
