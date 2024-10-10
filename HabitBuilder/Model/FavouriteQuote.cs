namespace HabitBuilder.Model
{
    public class FavouriteQuote
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<Quote> Quotes { get; set; } = new List<Quote>();
    }
}
