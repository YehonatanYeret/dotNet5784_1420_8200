namespace Dal;
using DalApi;

internal class ClockImplementation : IClock
{
    public DateTime? GetCurrentTime()
    {
        //return the current time stored in the configuration
        return DataSource.Config.CurrentTime;
    }

    public DateTime? GetStartProject()
    {
        //return the start project stored in the configuration
        return DataSource.Config.StartDate;
    }

    public void resetTimeLine()
    {
        //reset the start project and the current time with null
        DataSource.Config.StartDate = null;
        DataSource.Config.CurrentTime = null;
    }

    public void SetCurrentTime(DateTime currentTime)
    {
        //set the current time in the configuration
        DataSource.Config.CurrentTime = currentTime;
    }

    public void SetStartProject(DateTime startProject)
    {
        //set the start project in the configuration
        DataSource.Config.StartDate = startProject;
    }
}
