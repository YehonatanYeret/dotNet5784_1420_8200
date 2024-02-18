namespace BlImplementation;
using BlApi;
using BO;
using System;

internal class ClockImplementation : IClock
{
    // Data Access Layer instance
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// // Get the end of the project
    /// </summary>
    /// <returns> return The end of the project</returns>
    public DateTime? GetEndProject()=> _dal.Clock.GetEndProject();

    /// <summary>
    /// Get the status of the project
    /// </summary>
    /// <returns> return the status of the project</returns>
    public ProjectStatus GetProjectStatus()
    {
        if (GetEndProject() < DateTime.Now)
            return BO.ProjectStatus.Done;
        if (GetStartProject() < DateTime.Now)
            return BO.ProjectStatus.InProgress;
        return BO.ProjectStatus.NotStarted;
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
    /// Set the end of the project
    /// </summary>
    /// <param name="endProject"> The end of the project</param>
    /// <returns> return The end of the project</returns>
    public DateTime? SetEndProject(DateTime endProject)=> _dal.Clock.SetEndProject(endProject);

    /// <summary>
    ///  Set the start of the project
    /// </summary>
    /// <param name="startProject"> The start of the project</param>
    /// <returns> return The start of the project</returns>
    public DateTime? SetStartProject(DateTime startProject)=> _dal.Clock.SetStartProject(startProject);
}
