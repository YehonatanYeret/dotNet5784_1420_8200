namespace BlImplementation;
using BlApi;
using BO;
using System;

internal class ClockImplementation : IClock
{
    // Data Access Layer instance
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly IBl _bl;
    internal ClockImplementation(IBl bl) => _bl = bl;
    /// <summary>
    ///  gets the current time
    /// </summary>
    /// <returns></returns>
    public DateTime? GetCurrentTime()=> _dal.Clock.GetCurrentTime();

    /// <summary>
    /// Get the status of the project
    /// </summary>
    /// <returns> return the status of the project</returns>
    public ProjectStatus GetProjectStatus()
    {
        if (GetStartProject() > _bl.Time)
            return BO.ProjectStatus.NotStarted;

        return BO.ProjectStatus.InProgress;
    }

    /// <summary>
    /// Get the start of the project
    /// </summary>
    /// <returns> return The start of the project</returns>
    public DateTime? GetStartProject()=> _dal.Clock.GetStartProject();

    /// <summary>
    /// Reset the time line
    /// </summary>
    public void Reset()=>_dal.Clock.resetTimeLine();

    /// <summary>
    /// sets the current time.
    /// </summary>
    /// <param name="currentTime"></param>
    public void SetCurrentTime(DateTime currentTime)=> _dal.Clock.SetCurrentTime(currentTime);

    /// <summary>
    ///  Set the start of the project
    /// </summary>
    /// <param name="startProject"> The start of the project</param>
    /// <returns> return The start of the project</returns>
    public void SetStartProject(DateTime startProject)=> _dal.Clock.SetStartProject(startProject);
}
