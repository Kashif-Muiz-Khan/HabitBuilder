using HabitBuilder.Context;
using HabitBuilder.Model;
using HabitBuilder.Context;
using HabitBuilder.Model;
using Microsoft.EntityFrameworkCore;


namespace HabitBuilder.Context
{
    public class HabitLogProvider
    {
        private readonly DatabaseContext _context;

        public HabitLogProvider(DatabaseContext context)
        {
            _context = context;
        }



        public async Task<List<HabitLog>?> GetAllOrdersAsync()
        {
            // Return all orders
            return await _context.HabitLogs
                .Include(order => order.User)
                .Include(order => order.HabitItems)
                .ThenInclude(item => item.Habit)
                .OrderBy(order => order.Id)
                .ToListAsync();
        }

        public IQueryable<HabitLog> GetAllOrders()
        {
            // Return IQueryable<HabitLog>
            return _context.HabitLogs
                .Include(order => order.User)
                .Include(order => order.HabitItems)
                .ThenInclude(item => item.Habit)
                .OrderBy(order => order.Id);
        }


        public async Task CreateOrder(User user, IEnumerable<HabitItems> items)
        {
            // Create a new order
            var order = new HabitLog
            {
                User = user,
                HabitItems = items.ToList(), // Materialize the items into a list
                Day = DateOnly.FromDateTime(DateTime.Now)
            };

            // Calculate total points for the order
            var totalPoints = order.HabitItems.Sum(item => item.Habit.Point);
            order.TotalPoints = totalPoints;

            // Add the order to the database
            _context.HabitLogs.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(HabitLog order)
        {
            _context.HabitLogs.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(HabitLog order)
        {
            _context.HabitLogs.Update(order);
            await _context.SaveChangesAsync();
        }


        public async Task<HabitLog?> GetOrderAsync(int id)
        {
            // Return the order with the specified ID
            return await _context.HabitLogs
                .Include(order => order.User)
                .Include(order => order.HabitItems)
                .ThenInclude(item => item.Habit.Id)
                .Include(item => item.TotalPoints)
                .FirstOrDefaultAsync(order => order.Id == id);
        }


        public async Task<int> GetTotalOrdersForUserAsync(User user)
        {
            // Return the total number of orders for the specified user
            return await _context.HabitLogs
                .Where(order => order.User.Id == user.Id)
                .CountAsync();
        }





    }
}