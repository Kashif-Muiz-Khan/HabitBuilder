using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HabitBuilder.Model
{
    public class User : IdentityUser
    {

        public List<Habit> Habits { get; set; } = new List<Habit>();

        public List<HabitLog> HabitLogs { get; set; } = new List<HabitLog>();
    }
}
