using HabitBuilder.Context;
using HabitBuilder.Model;
using Microsoft.EntityFrameworkCore;

namespace HabitBuilder.Context
{
    public class HabitOrderProvider
    {
        private readonly DatabaseContext _context;

        public HabitOrderProvider(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<HabitOrder>?> GetAllOrdersAsync(User user)
        {
            // Return all orders for the specified user
            return await _context.Orders
                .Where(order => order.User.Id == user.Id) // Filter by user ID
                .Include(order => order.User)
                .Include(order => order.Items)
                .ThenInclude(item => item.Habit)
                .OrderBy(order => order.Id)
                .ToListAsync();
        }

        public IQueryable<HabitOrder> GetAllOrders(User user)
        {
            // Return IQueryable<Order> filtered by user ID
            return _context.Orders
                .Where(order => order.User.Id == user.Id) // Filter by user ID
                .Include(order => order.User)
                .Include(order => order.Items)
                .ThenInclude(item => item.Habit)
                .OrderBy(order => order.Id);
        }

        public async Task CreateOrder(User user, IEnumerable<HabitOrderItem> items)
        {
            // Create a new order
            var order = new HabitOrder
            {
                User = user,
                Items = items.ToList(), // Materialize the items into a list
                Day = DateOnly.FromDateTime(DateTime.Now)
            };

            // Calculate total points for the order
            var totalPoints = order.Items.Sum(item => item.Habit.Point);
            order.TotalPoints = totalPoints;

            // Add the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(HabitOrder order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(HabitOrder order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<HabitOrder?> GetOrderAsync(int id, User user)
        {
            // Return the order with the specified ID and user
            return await _context.Orders
                .Where(order => order.User.Id == user.Id && order.Id == id) // Filter by user ID and order ID
                .Include(order => order.User)
                .Include(order => order.Items)
                .ThenInclude(item => item.Habit)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalOrdersForUserAsync(User user)
        {
            // Return the total number of orders for the specified user
            return await _context.Orders
                .Where(order => order.User.Id == user.Id) // Filter by user ID
                .CountAsync();
        }
    }
}
