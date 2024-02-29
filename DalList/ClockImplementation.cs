namespace Dal;
using DalApi;

internal class ClockImplementation : IClock
{
    public DateTime? GetCurrentTime()
    {
        return DataSource.Config.CurrentTime;
    }

    public DateTime? GetStartProject()
    {
        return DataSource.Config.StartDate;
    }

    public void resetTimeLine()
    {
        DataSource.Config.StartDate = null;
        DataSource.Config.CurrentTime = null;
    }

    public void SetCurrentTime(DateTime currentTime)
    {
        DataSource.Config.CurrentTime = currentTime;
    }

    public void SetStartProject(DateTime startProject)
    {
        DataSource.Config.StartDate = startProject;
    }
}
