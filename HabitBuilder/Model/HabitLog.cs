
namespace HabitBuilder.Model
{
    public class HabitLog
    {
        public int Id { get; set; }
        public Habit Habit { get; set; }
        public User User { get; set; }

        public List<HabitItems> HabitItems { get; set; } = [];
        public DateOnly Day { get; set; }
        public int TotalPoints { get; set; }
    }
}
