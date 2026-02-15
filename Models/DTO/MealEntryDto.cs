using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitApp.App.Models.DTO
{
    public class MealEntryDto
    {
        public int MealEntryId { get; set; }
        public int Quantity { get; set; }
        public string MealName { get; set; } = string.Empty;
        public int MealCalories { get; set; }
        public int MealProtein { get; set; }
        public int MealCarbon { get; set; }
        public int MealFat { get; set; }
    }
}