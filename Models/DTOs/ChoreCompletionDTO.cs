namespace HouseRules.Models.DTOs;

public class ChoreCompletionDTO
{
    public int Id { get; set; }
    public int UserProfileId { get; set; }
    public int ChoreId { get; set; }
    public DateTime CompletedOn { get; set; }
    public ChoreDTO Chore { get; set; }
    public UserProfileDTO UserProfile { get; set; }
}