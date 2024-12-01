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

    // Retrieves all orders for a specified user
    public async Task<List<HabitOrder>?> GetAllOrdersAsync(User user)
    {
        return await _context.Orders
            .Where(order => order.User.Id == user.Id)
            .Include(order => order.User)
            .Include(order => order.Items)
            .ThenInclude(item => item.Habit)
            .OrderBy(order => order.Id)
            .ToListAsync();
    }

    // Retrieves all orders for a specified user as IQueryable
    public IQueryable<HabitOrder> GetAllOrders(User user)
    {
        return _context.Orders
            .Where(order => order.User.Id == user.Id)
            .Include(order => order.User)
            .Include(order => order.Items)
            .ThenInclude(item => item.Habit)
            .OrderBy(order => order.Id);
    }

    // Creates a new order for a user with the specified items
    public async Task CreateOrder(User user, IEnumerable<HabitOrderItem> items)
    {
        var order = new HabitOrder
        {
            User = user,
            Items = items.ToList(),
            Day = DateOnly.FromDateTime(DateTime.Now),
            TotalPoints = items.Sum(item => item.Habit.Point)
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

        public async Task CreateOrder(User user, List<HabitOrderItem> items, DateOnly day, int totalPoints)
        {
            var order = new HabitOrder
            {
                User = user,
                Items = items,
                Day = day,
                TotalPoints = totalPoints
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }



        // Deletes a specified order
        public async Task DeleteOrder(HabitOrder order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    // Updates a specified order
    public async Task UpdateOrderAsync(HabitOrder order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}
}
