namespace PL.Task;

using PL.Engineer;
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

    public BO.Engineer? currentEngineer { get; set; }

    /// <summary>
    /// Constructor for EngineerListWindow
    /// </summary>
    public TaskListWindow()
    {

        InitializeComponent();

    }

    public bool setupWindow(int id = 0)
    {
        if (this.Owner != null && this.Owner.Title == "EngineerShowWindow")
        {
            TaskList = (from t in s_bl.Task.ReadAllTask(task => task.Engineer != null && task.Engineer.Id == id && task.Status == BO.Status.Scheduled)
                        select (BlApi.Factory.Get().Task.ConvertToTaskInList(t.Id))).OrderBy(item => item.Id);

            if (!TaskList.Any())
            {
                MessageBox.Show("there are no task that you can choose", "feild in choose task to engineer",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            currentEngineer = s_bl.Engineer.Read(id);
            status = BO.Status.Scheduled;
        }
        else
        {
            TaskList = s_bl.Task.ReadAll().OrderBy(item => item.Id);
        }
        return true;
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
        new Task.TaskWindow().ShowDialog();
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

            if (this.Owner.Title == "ManagerWindow")
            {

                new Task.TaskWindow(taskInList!.Id).ShowDialog();
                UpdateListView();
            }

            if (this.Owner.Title == "EngineerShowWindow")
            {
                currentEngineer!.Task = new BO.TaskInEngineer { Id = taskInList.Id, Alias = taskInList.Alias };
                s_bl.Engineer.Update(currentEngineer);
                s_bl.Task.ChangeStatusOfTask(taskInList.Id);
                Close();
            }
        }
    }


    /// <summary>
    /// Updates the ListView based on the selected experience level
    /// </summary>
    void UpdateListView()
    {
        TaskList = (status == BO.Status.None) ?
            s_bl?.Task.ReadAll()!.OrderBy(item => item.Id)! :
            s_bl?.Task.ReadAll(item => item.Status == status)!.OrderBy(item => item.Id)!;
    }
}
