using HabitBuilder.Model;
using Microsoft.EntityFrameworkCore;

namespace HabitBuilder.Context
{
    public class ReviewProvider
    {
        private readonly DatabaseContext _context;

        public ReviewProvider(DatabaseContext context)
        {
            _context = context;
        }

        // Method to get total points for a user across all habit orders
        public async Task<int> GetTotalPointsForUserAsync(User user)
        {
            return await _context.Orders
                .Where(order => order.User.Id == user.Id)
                .SumAsync(order => order.TotalPoints);
        }

        // Method to get the total number of habits completed by a user
        public async Task<int> GetTotalHabitsCompletedAsync(User user)
        {
            return await _context.Orders
                .Where(order => order.User.Id == user.Id)
                .SelectMany(order => order.Items)
                .CountAsync();
        }

        // Method to get the longest streak for a particular habit
        public async Task<int> GetLongestStreakForHabitAsync(User user, int habitId)
        {
            // Fetch habit orders and sort them by date
            var logs = await _context.Orders
                .Where(order => order.User.Id == user.Id && order.Items.Any(item => item.Habit.Id == habitId))
                .OrderBy(order => order.Day)
                .ToListAsync();

            return CalculateLongestStreak(logs);
        }

        // Helper method to calculate the longest streak
        private int CalculateLongestStreak(List<HabitOrder> logs)
        {
            int currentStreak = 0;
            int longestStreak = 0;
            DateOnly? lastDate = null;

            foreach (var log in logs)
            {
                if (lastDate.HasValue && (log.Day.DayNumber - lastDate.Value.DayNumber) == 1)
                {
                    currentStreak++;
                }
                else
                {
                    currentStreak = 1;
                }

                if (currentStreak > longestStreak)
                {
                    longestStreak = currentStreak;
                }

                lastDate = log.Day;
            }

            return longestStreak;
        }

        // Method to get a detailed summary of habit performance for a user
        public async Task<List<HabitPerformance>> GetHabitPerformanceAsync(User user)
        {
            return await _context.Orders
                .Where(order => order.User.Id == user.Id)
                .SelectMany(order => order.Items)
                .GroupBy(item => item.Habit.Id)
                .Select(group => new HabitPerformance
                {
                    HabitId = group.Key,
                    HabitName = group.First().Habit.Name,
                    TotalCompletions = group.Count(),
                    TotalPoints = group.Sum(item => item.Habit.Point)
                }).ToListAsync();
        }
    }

    // Class to represent habit performance details
    public class HabitPerformance
    {
        public int HabitId { get; set; }
        public string HabitName { get; set; }
        public int TotalCompletions { get; set; }
        public int TotalPoints { get; set; }
    }
}
