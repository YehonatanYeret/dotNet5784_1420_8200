namespace BlImplementation;
using BlApi;
using BO;
using System;

internal class ClockImplementation : IClock
{
    // Data Access Layer instance
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public DateTime? GetEndProject()=> _dal.Clock.GetEndProject();

    public ProjectStatus GetProjectStatus()
    {
        if (GetEndProject() < DateTime.Now)
            return BO.ProjectStatus.Done;
        if (GetStartProject() < DateTime.Now)
            return BO.ProjectStatus.InProgress;
        return BO.ProjectStatus.NotStarted;
    }

    public DateTime? GetStartProject()=> _dal.Clock.GetStartProject();

    public void Reset()=>_dal.Clock.resetTimeLine();

    public DateTime? SetEndProject(DateTime endProject)=> _dal.Clock.SetEndProject(endProject);

    public DateTime? SetStartProject(DateTime startProject)=> _dal.Clock.SetStartProject(startProject);
}
