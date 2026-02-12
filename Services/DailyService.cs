using System.Net.Http.Json;
using FitApp.App.Models;
using FitApp.App.Models.DTO;

public class DailyService
{
    private readonly HttpClient _http;

    public DailyService(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://fitappapi.azurewebsites.net/api/");
    }

    public async Task<int?> CreateDailyAsync(int userId, DateTime date)
    {
        var dto = new CreateDailyReportDto
        {
            UserId = userId,
            DailyReportDate = date.Date
        };

        var response = await _http.PostAsJsonAsync("daily", dto);

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            Console.WriteLine(err);
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<DailyReport>();
        return result?.Id;
    }

    public async Task<DailyReport?> GetDailyByDateAsync(int userId, DateTime date)
    {
        var res = await _http.GetAsync(
            $"daily/user/{userId}/date/{date:yyyy-MM-dd}");

        if (!res.IsSuccessStatusCode)
            return null;

        return await res.Content.ReadFromJsonAsync<DailyReport>();
    }


}
