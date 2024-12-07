using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HabitBuilder.Model
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public List<Habit> Habits { get; set; } = new List<Habit>();

        public List<HabitOrder> Orders { get; set; } = [];

        public List<FavouriteQuote> FavouriteQuotes { get; set; } = new List<FavouriteQuote>();
    }
}
