using System.Net.Http.Json;
using FitApp.App.Models;

namespace FitApp.App.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://fitappapi.azurewebsites.net/api/");
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            Console.WriteLine($"Email: '{email}'");
            Console.WriteLine($"Password: '{password}'");

            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("user/login", loginRequest);

            if (!response.IsSuccessStatusCode)
                return null;

            var user = await response.Content.ReadFromJsonAsync<User>();
            return user;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("user/register", user);

                if (response.IsSuccessStatusCode)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Register error: {ex.Message}");
                return false;
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"user/id?id={userId}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<User>();
        }



    }
}
