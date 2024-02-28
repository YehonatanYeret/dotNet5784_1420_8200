namespace BO;

/// <summary>
/// Class of Engineer
/// </summary>
public class Engineer
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double Cost { get; set; }
    public TaskInEngineer? Task { get; set; }
    //bonus
    public string? Image { get; set; }
    //bonus
    public override string ToString()=>this.ToStringProperty();
}
