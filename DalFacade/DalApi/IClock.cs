namespace DalApi;

/// <summary>
/// creates an interface for the timeline
/// </summary>
public interface IClock
{
    /// <summary>
    /// sets the start project
    /// </summary>
    /// <param name="startProject"></param>
    public void SetStartProject(DateTime startProject);

    /// <summary>
    /// gets the start project
    /// </summary>
    /// <returns></returns>
    public DateTime? GetStartProject();

    /// <summary>
    /// gets the current time
    /// </summary>
    /// <returns></returns>
    public DateTime? GetCurrentTime();

    /// <summary>
    /// sets the current time
    /// </summary>
    /// <param name="currentTime"></param>
    public void SetCurrentTime(DateTime currentTime);

    /// <summary>
    /// resets the timeline
    /// </summary>
    public void resetTimeLine();
}