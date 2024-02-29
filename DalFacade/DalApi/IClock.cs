namespace DalApi;

/// <summary>
/// creates an interface for the timeline
/// </summary>
public interface IClock
{
    public void SetStartProject(DateTime startProject);
    public DateTime? GetStartProject();

    public DateTime? GetCurrentTime();
    public void SetCurrentTime(DateTime currentTime);

    public void resetTimeLine();
}