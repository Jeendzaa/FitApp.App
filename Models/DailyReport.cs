namespace FitApp.App.Models
{
    public class DailyReport
    {
        public int Id { get; set; }
        public DateTime DailyReportDate { get; set; }
        public int UserId { get; set; }

        public List<MealEntry> MealEntries { get; set; } = new();

        public int DailyReportCalories { get; set; }
        public double? DailyReportFat { get; set; }
        public double? DailyReportCarbon { get; set; }
        public double? DailyReportProtein { get; set; }
        public double? DailyReportWater { get; set; }
        public double DailyReportWeight { get; set; }
    }

    public class Meal
    {
        public int MealId { get; set; }
        public string MealName { get; set; } = string.Empty;
        public int MealCalories { get; set; }
        public int MealProtein { get; set; }
        public int MealCarbon { get; set; }
        public int MealFat { get; set; }
    }

    public class MealEntry
    {
        public int MealEntryId { get; set; }
        public int MealEntryQuantity { get; set; }
        public DateTime MealEntryDate { get; set; }

        public int MealId { get; set; }
        public Meal Meal { get; set; }

        public int DailyReportId { get; set; }
    }
}
