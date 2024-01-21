using System.ComponentModel.DataAnnotations;
namespace HouseRules.Models;


public class Chore
{
    // public Chore(string name, int difficulty, int choreFrequencyDays)
    // {
    //     Name = name;
    //     Difficulty = difficulty;
    //     ChoreFrequencyDays = choreFrequencyDays;
    // }
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int Difficulty { get; set; }
    public int ChoreFrequencyDays { get; set; }
    public List<ChoreAssignment> ChoreAssignments { get; set; }
    public List<ChoreCompletion> ChoreCompletions { get; set; }
    
}