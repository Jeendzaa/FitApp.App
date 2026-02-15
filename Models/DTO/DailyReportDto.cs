using System.Text.Json.Serialization;

namespace FitApp.App.Models.DTO
{
    public class DailyReportDto
    {
        public int Id { get; set; }
        public DateTime DailyReportDate { get; set; }

        [JsonPropertyName("dailyReportCalories")]
        public int Calories { get; set; }

        [JsonPropertyName("dailyReportProtein")]
        public double? Protein { get; set; }

        [JsonPropertyName("dailyReportCarbon")]
        public double? Carbon { get; set; }

        [JsonPropertyName("dailyReportFat")]
        public double? Fat { get; set; }

        [JsonPropertyName("dailyReportWater")]
        public double? Water { get; set; }

        [JsonPropertyName("meals")]
        public List<MealEntryDto> Meals { get; set; } = new();
    }
}
