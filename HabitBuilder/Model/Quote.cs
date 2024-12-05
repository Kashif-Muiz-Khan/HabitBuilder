using System.ComponentModel.DataAnnotations;

namespace HabitBuilder.Model
{
    public class Quote // Quote model
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Quote")]
        public string QuoteText { get; set; }

        [Required(ErrorMessage = "Please enter the Author")]
        public string Author { get; set; }
        public FavouriteQuote? FavouiteQuote { get; set; } // Links the quote as favourited to a certain user
    }
}
