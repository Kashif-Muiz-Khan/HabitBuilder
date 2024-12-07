namespace HabitBuilder.Model
{
    public class HabitPerformance
    {
        public int HabitId { get; set; }
        public string HabitName { get; set; } = string.Empty;
        public int TotalCompletions { get; set; } // Fix for the error
        public int CompletedCount { get; set; } // Number of times the habit was completed
        public int TotalPoints { get; set; } // Points for completed habits
        public int Streak { get; set; } // Current streak
        public int MissedCount { get; set; } // Count of missed habits
        public int RequiredCount { get; set; } // Target habit count
    }
}
