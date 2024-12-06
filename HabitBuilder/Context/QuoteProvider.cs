using HabitBuilder.Model;
using Microsoft.EntityFrameworkCore;

namespace HabitBuilder.Context
{
    public class QuoteProvider
    {
        private readonly DatabaseContext _context;

        public QuoteProvider(DatabaseContext context)
        {
            _context = context;
        }

        // Method to get all quotes, ordered by QuoteText
        public async Task<List<Quote>> GetAllQuotesAsync()
        {
            return await _context.Quotes.OrderBy(quote => quote.QuoteText).ToListAsync();
        }

        // Method to add a new quote
        public async Task AddQuoteAsync(Quote quote)
        {
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();
        }

        // Method to delete a quote
        public async Task DeleteQuoteAsync(Quote quote)
        {
            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
        }

        // Method to update an existing quote
        public async Task UpdateQuoteAsync(Quote quote)
        {
            _context.Quotes.Update(quote);
            await _context.SaveChangesAsync();
        }

        // Optional: Method to get a specific quote by Id
        public async Task<Quote?> GetQuoteByIdAsync(int id)
        {
            return await _context.Quotes.FirstOrDefaultAsync(q => q.Id == id);
        }



        public async Task ToggleFavoriteAsync(int quoteId, User userId)
        {
            // Retrieve the user and their favorite quotes list
            var favoriteQuote = await _context.FavouriteQuotes
                .Include(f => f.Quotes)
                .FirstOrDefaultAsync(f => f.User == userId);

            // Retrieve the quote
            var quote = await _context.Quotes.FirstOrDefaultAsync(q => q.Id == quoteId);
            if (quote == null)
                throw new Exception("Quote not found.");

            if (favoriteQuote == null)
            {
                // Create a new favorite list for the user if it doesn't exist
                favoriteQuote = new FavouriteQuote
                {
                    User = userId,
                    

                };
                _context.FavouriteQuotes.Add(favoriteQuote);
                await _context.SaveChangesAsync();
            }

            // Check if the quote is already in the user's favorite list
            if (favoriteQuote.Quotes.Any(q => q.Id == quoteId))
            {
                // Remove the quote if it is already a favorite
                favoriteQuote.Quotes.Remove(quote);
            }
            else
            {
                // Add the quote if it is not a favorite
                favoriteQuote.Quotes.Add(quote);
            }

            // Save the changes
            await _context.SaveChangesAsync();
        }


        public async Task<Quote?> GetRandomQuoteAsync()
        {
            // Retrieve all quotes
            var quotes = await _context.Quotes.ToListAsync();

            // Return null if no quotes are available
            if (quotes == null || quotes.Count == 0)
                return null;

            // Select a random quote
            var random = new Random();
            int index = random.Next(quotes.Count);
            return quotes[index];
        }



    }
}
