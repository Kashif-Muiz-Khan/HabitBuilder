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
    }
}
