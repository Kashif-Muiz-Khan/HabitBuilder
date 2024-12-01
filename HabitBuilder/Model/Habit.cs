using System.ComponentModel.DataAnnotations;

namespace HabitBuilder.Model
{
    public class Habit
    {
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter the Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please enter the Type")]
    public string Type { get; set; } = "";

    [Required(ErrorMessage = "Please enter the Description")]
    public string Description { get; set; }

    [Required(ErrorMessage ="Please choose the frequency")]
    public FrequencyLevel Frequency { get; set; }

    [Required(ErrorMessage = "Please choose the target ")]
    public int Target {  get; set; }

    [Required(ErrorMessage = "Please choose the difficulty")]
        public int Point => (int)Difficulty;

    [Required(ErrorMessage = "Please choose the difficulty")]
    public DifficultyLevel Difficulty { get; set; }

     public bool IsChecked { get; set; }

    public User User { get; set; } 

    }


    public enum DifficultyLevel
    {
        VeryEasy = 2,
        Easy = 5,
        Medium = 8,
        Hard = 12,
        VeryHard = 16,
        Impossible = 25
    }

    public enum FrequencyLevel
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly

    }

}

