namespace PL;

using System.Windows;

/// <summary>
/// Represents the application's entry point and lifecycle events.
/// </summary>
public partial class App : Application
{
    // Get the instance of the business logic layer
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Event handler called when the application is exiting.
    /// </summary>
    /// <param name="e">Exit event arguments.</param>
    protected override void OnExit(ExitEventArgs e)
    {
        // Set the current time in the business logic layer before exiting
        s_bl.Clock.SetCurrentTime(s_bl.Time);
        // Call the base class method for further processing
        base.OnExit(e);
    }

    /// <summary>
    /// Event handler called when the application is starting.
    /// </summary>
    /// <param name="e">Startup event arguments.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        // Call the base class method for further processing
        base.OnStartup(e);
        s_bl.Init();
    }
}
