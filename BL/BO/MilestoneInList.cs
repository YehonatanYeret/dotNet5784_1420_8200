namespace BO;

/// <summary>
/// presents a milestone in a list
/// </summary>
public class MilestoneInList
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public Status? Status { get; set; }
    public double? CompletionPercentage { get; set; }
}
