namespace PL.Manager;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for ManagerWindow.xaml
/// </summary>
public partial class ManagerWindow : Window
{
    public Visibility IsprojectStarted { get; set; }
    // Business logic layer instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public ManagerWindow()
    {
        IsprojectStarted = (s_bl.Clock.GetProjectStatus() == BO.ProjectStatus.InProgress)? Visibility.Hidden: Visibility.Visible;
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
            s_bl.ResetDB();
            IsprojectStarted = Visibility.Hidden;
        }

    }

    private void InitializeDataBtn_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to initialization the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
        {
            s_bl.InitializeDB();
            IsprojectStarted = Visibility.Visible;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new GantWindow().Show();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var dialogBox = new Messeges.PickDate();
        if (dialogBox.ShowDialog() == true)
        {
            DateTime startProject = (DateTime)dialogBox.Date!;

            foreach (var item in s_bl.Task.ReadAllTask(task => task.ScheduledDate == null))
            {

                // calculate the closest date based on the startDate of the program and the depndencies 
                DateTime closest = s_bl.Task.CalculateClosestStartDate(item.Id, startProject);
                s_bl.Task.UpdateScheduledDate(item.Id, closest);
            }
            s_bl.Clock.SetStartProject(startProject);
        }
        (sender as Button)!.Visibility = Visibility.Hidden;
    }
}
