using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HabitBuilder.Model
{
    public class User : IdentityUser
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Please enter the First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the Last Name")]
        public string LastName { get; set; }

        public List<Habit> Habits { get; set; } = new List<Habit>();
    }
}
