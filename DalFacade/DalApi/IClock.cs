namespace DalApi;
using DO;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// creates an interface for the timeline
/// </summary>
public interface IClock
{
    public DateTime? SetStartProject(DateTime startProject);
    public DateTime? GetStartProject();

    public DateTime? SetEndProject(DateTime endProject);
    public DateTime? GetEndProject();

    public void resetTimeLine();
}