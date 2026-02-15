using System.Net.Http.Json;
using FitApp.App.Models.DTO;

namespace FitApp.App.Services
{
    public class MealService
    {
        private readonly HttpClient _http;

        public MealService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://fitappapi.azurewebsites.net/api/");
        }

        public async Task<List<MealDto>> GetAllMealsAsync()
        {
            var response = await _http.GetAsync("meal");

            if (!response.IsSuccessStatusCode)
                return new List<MealDto>();

            var meals = await response.Content.ReadFromJsonAsync<List<MealDto>>();
            return meals ?? new List<MealDto>();
        }

        public async Task<List<MealDto>> SearchMealsAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await GetAllMealsAsync();

            var response = await _http.GetAsync($"meal/by-name/{name}");

            if (!response.IsSuccessStatusCode)
                return new List<MealDto>();

            var meals = await response.Content.ReadFromJsonAsync<List<MealDto>>();
            return meals ?? new List<MealDto>();
        }
    }
}
