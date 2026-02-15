using System.Net.Http.Json;
using FitApp.App.Models.DTO;

namespace FitApp.App.Services
{
    public class DailyService
    {
        private readonly HttpClient _http;

        public DailyService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://fitappapi.azurewebsites.net/api/");
        }

        public async Task<DailyReportDto?> GetDailyByDateAsync(int userId, DateTime date)
        {
            var res = await _http.GetAsync(
                $"daily/user/{userId}/date/{date:yyyy-MM-dd}");

            if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (!res.IsSuccessStatusCode)
                return null;

            return await res.Content.ReadFromJsonAsync<DailyReportDto>();
        }

        public async Task<int?> CreateDailyAsync(int userId, DateTime date)
        {
            var dto = new
            {
                UserId = userId,
                DailyReportDate = date.Date
            };

            var response = await _http.PostAsJsonAsync("daily", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<int>();
        }
        public async Task<bool> UpdateWaterAsync(int id, double water)
        {
            var body = new
            {
                DailyReportWater = water
            };

            var response = await _http.PutAsJsonAsync($"daily/{id}/water", body);

            return response.IsSuccessStatusCode;
        }
    }
}
