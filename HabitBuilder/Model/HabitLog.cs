
namespace HabitBuilder.Model
{
    public class HabitLog
    {
        public int Id { get; set; }
        public Habit Habit { get; set; }
        public bool HabitStatus { get; set; }
        public DateTime Date { get; set; }
    }
}
