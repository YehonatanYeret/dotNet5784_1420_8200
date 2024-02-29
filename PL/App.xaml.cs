namespace PL;

using System.Windows;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application 
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    protected override void OnExit(ExitEventArgs e)
    {
        s_bl.Clock.SetCurrentTime(s_bl.Time);
        base.OnExit(e);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DateTime? time = s_bl.Clock.GetCurrentTime();
        if (time != null)
            s_bl.Init(time.Value);
    }
}
