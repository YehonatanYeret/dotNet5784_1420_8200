namespace PL.Task;

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

    /// <summary>
    /// Constructor for EngineerListWindow
    /// </summary>
    public TaskListWindow()
    {
        InitializeComponent();
        TaskList = s_bl.Task.ReadAll()!;
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
        new Task.TaskWindow(taskInList!.Id).ShowDialog();
        UpdateListView();
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
