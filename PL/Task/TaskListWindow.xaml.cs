namespace PL.Task;

using BO;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    // Business logic layer instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Dependency Property for list of Tasks
    /// </summary>
    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    /// <summary>
    /// Dependency property for TaskList
    /// </summary>
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Selected task's status
    /// </summary>
    public BO.Status status { get; set; } = BO.Status.None;

    public BO.Engineer? currentEngineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("currentEngineer", typeof(BO.Engineer), typeof(TaskListWindow), new PropertyMetadata(null));

    public int EngineerID { get; set; }


    public bool IsProjectStarted
    {
        get { return (bool)GetValue(IsProjectStartedProperty); }
        set { SetValue(IsProjectStartedProperty, value); }
    }

    public static readonly DependencyProperty IsProjectStartedProperty =
        DependencyProperty.Register("IsProjectStarted", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));


    /// <summary>
    /// Constructor for EngineerListWindow
    /// </summary>
    public TaskListWindow(int engineerID = 0)
    {   
        EngineerID = engineerID;
        IsProjectStarted = s_bl.Clock.GetProjectStatus() == BO.ProjectStatus.InProgress;

        if (EngineerID == 0)
        {
            TaskList = s_bl.Task.ReadAll().OrderBy(item => item.Id);
        }
        else
        {
            TaskList = s_bl.Engineer.GetTasksOfEngineer(EngineerID);
            currentEngineer = s_bl.Engineer.Read(EngineerID);
            status = BO.Status.Scheduled;
        }
        InitializeComponent();

    }


    /// <summary>
    /// Event handler for selection changed in the ComboBox
    /// </summary>
    private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateListView();
    }

    /// <summary>
    /// Event handler for the "Add" button click
    /// </summary>
    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
        new TaskWindow().ShowDialog();
        UpdateListView();
    }

    /// <summary>
    /// Event handler for double click on the ListView item
    /// </summary>
    private void UpdateListView_DoubleClick(object sender, RoutedEventArgs e)
    {
        BO.TaskInList? taskInList = (sender as ListView)?.SelectedItem as BO.TaskInList;
        if (taskInList != null)
        {

            if (EngineerID == 0)
            {
                //new Task.TaskWindow(taskInList!.Id).ShowDialog();
                new TaskWindow(taskInList!.Id).ShowDialog();
                UpdateListView();
            }
            else
            {
                try
                {
                    currentEngineer!.Task = new BO.TaskInEngineer { Id = taskInList.Id, Alias = taskInList.Alias };
                    s_bl.Task.ChangeStatusOfTask(taskInList.Id);
                    s_bl.Engineer.Update(currentEngineer);
                    Close();
                }
                catch (BLValueIsNotCorrectException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }


    /// <summary>
    /// Updates the ListView based on the selected experience level
    /// </summary>
    void UpdateListView()
    {
        if (EngineerID == 0)
        {
            TaskList = (status == BO.Status.None) ?
                s_bl?.Task.ReadAll()!.OrderBy(item => item.Id)! :
                s_bl?.Task.ReadAll(item => item.Status == status)!.OrderBy(item => item.Id)!;
        }
        else
        {
            TaskList = (status == BO.Status.None) ?
                s_bl?.Engineer.GetTasksOfEngineer(EngineerID)!.OrderBy(item => item.Id)! :
                s_bl?.Engineer.GetTasksOfEngineer(EngineerID).Where(item => item.Status == status)!.OrderBy(item => item.Id)!;
        }
    }
}
