namespace HabitBuilder.Model
{
    public class HabitOrderItem
    {
        public int Id { get; set; }
        public HabitOrder Order { get; set; }
        public Habit Habit { get; set; }


    }
}
