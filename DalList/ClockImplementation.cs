namespace Dal;
using DalApi;

internal class ClockImplementation : IClock
{
    public DateTime? GetEndProject()
    {
        return DataSource.Config.EndDate;
    }

    public DateTime? GetStartProject()
    {
        return DataSource.Config.StartDate;
    }

    public void resetTimeLine()
    {
        DataSource.Config.StartDate = null;
        DataSource.Config.EndDate = null;
    }

    public DateTime? SetEndProject(DateTime endProject)
    {
        DataSource.Config.EndDate = endProject;
        return endProject;
    }

    public DateTime? SetStartProject(DateTime startProject)
    {
        DataSource.Config.StartDate = startProject;
        return startProject;
    }
}
