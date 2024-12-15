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

        // Get total points for a user across all habit orders
        public async Task<int> GetTotalPointsForUserAsync(User user)
        {
            return await _context.Orders
                .Where(order => order.User.Id == user.Id)
                .SumAsync(order => order.TotalPoints);
        }

        // Get total habits completed by a user
        public async Task<int> GetTotalHabitsCompletedAsync(User user)
        {
            return await _context.Orders
                .Where(order => order.User.Id == user.Id)
                .SelectMany(order => order.Items)
                .CountAsync();
        }

        // Get the longest streak for a specific habit
        public async Task<int> GetLongestStreakForHabitAsync(User user, int habitId)
        {
            var logs = await _context.Orders
                .Where(order => order.User.Id == user.Id && order.Items.Any(item => item.Habit.Id == habitId))
                .OrderBy(order => order.Day)
                .ToListAsync();

            return CalculateLongestStreak(logs);
        }

        // Get the highest streak across all habits
        public async Task<int> GetHighestStreakAsync(User user)
        {
            var habitPerformances = await GetHabitPerformanceAsync(user);

            // Check if habitPerformances is empty
            if (!habitPerformances.Any())
            {
                return 0; // Return 0 or a default value when there are no habits
            }

            return habitPerformances.Max(hp => hp.Streak);
        }


        // Get the best-performing habit(s)
        public async Task<List<HabitPerformance>> GetBestPerformingHabitAsync(User user)
        {
            var habitPerformances = await GetHabitPerformanceAsync(user);

            // Check if habitPerformances is empty
            if (!habitPerformances.Any())
            {
                return new List<HabitPerformance>(); // Return an empty list when there are no habits
            }

            var maxPoints = habitPerformances.Max(hp => hp.TotalPoints);
            return habitPerformances.Where(hp => hp.TotalPoints == maxPoints).ToList();
        }


        // Get the worst-performing habit(s)
        public async Task<List<HabitPerformance>> GetWorstPerformingHabitAsync(User user)
        {
            var habitPerformances = await GetHabitPerformanceAsync(user);
            var minPoints = habitPerformances.Min(hp => hp.TotalPoints);
            return habitPerformances.Where(hp => hp.TotalPoints == minPoints).ToList();
        }

        // Calculate the average streak for a user
public async Task<double> GetAverageStreakAsync(User user)
{
    // Retrieve the habit performances for the user
    var habitPerformances = await GetHabitPerformanceAsync(user);

    // Check if the sequence is empty to avoid InvalidOperationException
    if (!habitPerformances.Any())
    {
        return 0; // Return 0 or an appropriate default value for an empty sequence
    }

    // Calculate and return the average streak
    return habitPerformances.Average(hp => hp.Streak);
}


        // Helper: Calculate longest streak
        private int CalculateLongestStreak(List<HabitOrder> logs)
        {
            int currentStreak = 0;
            int longestStreak = 0;
            DateOnly? lastDate = null;

            foreach (var log in logs)
            {
                if (lastDate.HasValue && (log.Day.DayNumber - lastDate.Value.DayNumber) == 1)
                {
                    currentStreak++; //Adds 1 to the streak if it was completed the day before as well
                }
                else
                {
                    currentStreak = 1; //If not then sets streak as 1
                }

                longestStreak = Math.Max(longestStreak, currentStreak);
                lastDate = log.Day;
            }

            return longestStreak;
        }

        // Get habit performance summary for a user
        // Retrieves a summary of habit performance for a specific user
        public async Task<List<HabitPerformance>> GetHabitPerformanceAsync(User user)
        {
            // Fetch all orders for the user, including related habit data
            var orders = await _context.Orders
                .Include(o => o.Items) // Include order items and their associated habits
                    .ThenInclude(i => i.Habit)
                .Where(o => o.User.Id == user.Id) // Filter by the user's ID
                .ToListAsync();

            // Process and group habit data to calculate performance metrics
            var habitPerformances = orders
                .SelectMany(order => order.Items) 
                .GroupBy(item => item.Habit.Id) // Group by habit ID
                .Select(group => new HabitPerformance
                {
                    HabitId = group.Key,
                    HabitName = group.First().Habit.Name,
                    TotalCompletions = group.Count(), // Total times the habit was completed
                    TotalPoints = group.Sum(item => item.Habit.Point), // Total points earned
                    Streak = CalculateStreak(group.ToList()), // Longest streak for this habit
                    MissedCount = group.Count(item => !item.Habit.IsChecked), // Missed occurrences
                    RequiredCount = group.First().Habit.Target // Target completion count
                }).ToList();

            return habitPerformances; // Return the calculated performance data
        }


        // Helper method for calculating streaks
        private int CalculateStreak(List<HabitOrderItem> logs)
        {
            logs = logs.OrderByDescending(item => item.Order.Day).ToList();
            int streak = 0;

            foreach (var log in logs)
            {
                if (log.Habit.IsChecked) streak++;
                else break;
            }

            return streak;
        }
    }
}
