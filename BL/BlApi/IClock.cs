namespace BlApi;

public interface IClock
{
    public DateTime? SetStartProject(DateTime startProject);
    public DateTime? GetStartProject();

    public DateTime? SetEndProject(DateTime endProject);
    public DateTime? GetEndProject();

    public  BO.ProjectStatus GetProjectStatus()
    {
        if(GetStartProject() < DateTime.Now)
            return BO.ProjectStatus.NotStarted;
        if (GetEndProject() < DateTime.Now)
            return BO.ProjectStatus.InProgress;
        return BO.ProjectStatus.Done;
    }
}
