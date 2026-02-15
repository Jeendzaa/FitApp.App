namespace FitApp.App.Models.DTO
{
    public class MealDto
    {
        public int MealId { get; set; }
        public string MealName { get; set; } = string.Empty;
        public int MealCalories { get; set; }
        public int MealProtein { get; set; }
        public int MealCarbon { get; set; }
        public int MealFat { get; set; }
    }
}
