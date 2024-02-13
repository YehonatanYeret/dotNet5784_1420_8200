namespace PL.Manager;

using System.Windows;

/// <summary>
/// Interaction logic for ManagerWindow.xaml
/// </summary>
public partial class ManagerWindow : Window
{
    // Business logic layer instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public ManagerWindow()
    {
        InitializeComponent();
    }

    private void EngineerButton_Click(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerListWindow().Show();
    }

    private void TaskButton_Click(object sender, RoutedEventArgs e)
    {
        new Task.TaskListWindow().Show();
    }

    private void ResetDataBtn_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to reset all the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
        {
            s_bl.Engineer.Reset();
            s_bl.Task.Reset();
            s_bl.Clock.Reset();
        }

    }

    private void InitializeDataBtn_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to initialization the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
        {
            DalTest.Initialization.Do();
        }
    }
}
