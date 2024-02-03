namespace BlImplementation;
using BlApi;
using BO;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IClock Clock => new ClockImplementation();
}
