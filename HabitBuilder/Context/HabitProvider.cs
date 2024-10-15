using HabitBuilder.Model;
using Microsoft.EntityFrameworkCore;

namespace HabitBuilder.Context
{
    public class HabitProvider
    {
        private readonly DatabaseContext _context;

        public HabitProvider(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Habit>> GetAllHabitsAsync()
        {
            return await _context.Habits.OrderBy(habits => habits.Name).ToListAsync();
        }

        public async Task AddHabitAsync(Habit habits)
        {
            _context.Habits.Add(habits);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHabitAsync(Habit habits)
        {
            _context.Habits.Remove(habits);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHabitAsync(Habit habits)
        {
            _context.Habits.Update(habits);
            await _context.SaveChangesAsync();
        }
    }
}
