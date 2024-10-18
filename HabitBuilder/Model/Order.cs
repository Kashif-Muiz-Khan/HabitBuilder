namespace HabitBuilder.Model
{
    public class HabitOrder
    {
        public int Id { get; set; }

        public User User { get; set; }
        public List<HabitOrderItem> Items { get; set; } = [];
        public DateOnly Day { get; set; }
        public int TotalPoints {  get; set; }
    }

}
