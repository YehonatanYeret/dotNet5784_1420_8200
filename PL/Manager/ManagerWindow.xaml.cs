namespace PL.Manager;

using PL.Task;
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
        TaskListWindow taskListWindow = new TaskListWindow();
        taskListWindow.Owner = this;
        taskListWindow.setupWindow();
        taskListWindow.Show();
    }

    private void ResetDataBtn_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to reset all the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
        {
            s_bl.ResetDB();
        }

    }

    private void InitializeDataBtn_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to initialization the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
        {
            s_bl.InitializeDB();
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new GantWindow().Show();
    }
}
