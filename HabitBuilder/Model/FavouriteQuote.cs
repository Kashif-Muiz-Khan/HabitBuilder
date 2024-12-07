namespace HabitBuilder.Model
{
    public class FavouriteQuote
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Quote Quote { get; set; }
    }
}
