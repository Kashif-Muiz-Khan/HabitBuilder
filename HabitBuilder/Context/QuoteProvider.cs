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
            return await _context.Quotes.OrderBy(quote => quote.Id).ToListAsync();
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

        // Method to get a specific quote by Id
        public async Task<Quote?> GetQuoteByIdAsync(int id)
        {
            return await _context.Quotes.FirstOrDefaultAsync(q => q.Id == id);
        }

        // Method to toggle a favorite quote for a user
        public async Task ToggleFavoriteAsync(int quoteId, User user)
        {
            // Check if the quote exists
            var quote = await _context.Quotes.FirstOrDefaultAsync(q => q.Id == quoteId);
            if (quote == null)
            {
                throw new Exception("Quote not found.");
            }

            // Check if the user already has this quote as a favorite
            var favoriteQuote = await _context.FavouriteQuotes
                .FirstOrDefaultAsync(fq => fq.Quote.Id == quoteId && fq.User.Id == user.Id);

            if (favoriteQuote == null)
            {
                // Add to favorites if not already present
                var newFavorite = new FavouriteQuote
                {
                    User = user,
                    Quote = quote
                };
                _context.FavouriteQuotes.Add(newFavorite);
            }
            else
            {
                // Remove from favorites if already present
                _context.FavouriteQuotes.Remove(favoriteQuote);
            }

            await _context.SaveChangesAsync();
        }

        // Method to retrieve a random quote
        public async Task<Quote?> GetRandomQuoteAsync()
        {
            // Retrieve all quotes
            var quotes = await _context.Quotes.ToListAsync();

            // Return null if no quotes are available
            if (quotes == null || quotes.Count == 0)
            {
                return null;
            }

            // Select a random quote
            var random = new Random();
            int index = random.Next(quotes.Count);
            return quotes[index];
        }

        // Method to retrieve all favorite quotes for a user
        public async Task<List<Quote>> GetFavoriteQuotesAsync(User user)
        {
            // Retrieve the user's favorite quotes
            return await _context.FavouriteQuotes
                .Where(fq => fq.User.Id == user.Id)
                .Include(fq => fq.Quote) // Include the Quote details
                .Select(fq => fq.Quote) // Select only the quotes
                .ToListAsync();
        }
    }
}
