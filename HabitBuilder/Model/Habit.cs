using System.ComponentModel.DataAnnotations;

namespace HabitBuilder.Model
{
    public class Habit
    {
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter the Name")] // Adds validation ensuring this is not null when saved to database
    public string Name { get; set; }

    [Required(ErrorMessage = "Please enter the Type")]
    public string Type { get; set; } = "";

    [Required(ErrorMessage = "Please enter the Description")]
    public string Description { get; set; }

    [Required(ErrorMessage ="Please choose the frequency")]
    public FrequencyLevel Frequency { get; set; } // Frequency has to be of type frequency which is the list of predefined frequencies

    [Required(ErrorMessage = "Please choose the target ")]
    public int Target {  get; set; }

    [Required(ErrorMessage = "Please choose the difficulty")]
        public int Point => (int)Difficulty; // The points have to be the associated points of frequencies that are predefined

    [Required(ErrorMessage = "Please choose the difficulty")]
    public DifficultyLevel Difficulty { get; set; }

     public bool IsChecked { get; set; }

    public User User { get; set; } 

    }


    // Predefines the difficulty levels and the associated points
    public enum DifficultyLevel
    {
        VeryEasy = 2,
        Easy = 5,
        Medium = 8,
        Hard = 12,
        VeryHard = 16,
        Impossible = 25
    }

    //Predefines the frequency levels for the user to choose from
    public enum FrequencyLevel
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly

    }

}

