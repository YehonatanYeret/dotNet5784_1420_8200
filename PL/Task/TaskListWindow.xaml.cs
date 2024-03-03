namespace PL.Task;

using BO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window, INotifyPropertyChanged
{
    // Business logic layer instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Event for property changed
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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
    /// Selected task's level
    /// </summary>
    public BO.EngineerExperience complexity { get; set; } = BO.EngineerExperience.None;

    /// <summary>
    /// Dependency property for the engineer of the task (if we are in engineer mode)
    /// </summary>
    public BO.Engineer? currentEngineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("currentEngineer", typeof(BO.Engineer), typeof(TaskListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property for the engineer id (if we are in engineer mode)
    /// </summary>
    public int EngineerID { get; set; }

    /// <summary>
    /// Dependency property for knowing if we are in engineer mode then we can choose a mission or just see the tasks
    /// </summary>
    public bool ChooseMission { get; set; }

    /// <summary>
    /// Dependency property for the recovery mode when 0 is recovery mode and 1 is normal mode
    /// </summary>
    public int NormalMode
    {
        get { return (int)GetValue(RecoveryModeProperty); }
        set
        {
            SetValue(RecoveryModeProperty, value);
            OnPropertyChanged(nameof(NormalMode));
        }
    }

    public static readonly DependencyProperty RecoveryModeProperty =
        DependencyProperty.Register("NormalMode", typeof(int), typeof(TaskListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Dependency property for the project status
    /// </summary>
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
    public TaskListWindow(int engineerID = 0, bool chooseMission = true)
    {
        // Initialize properties based on parameters and business logic
        ChooseMission = chooseMission;
        EngineerID = engineerID;
        IsProjectStarted = s_bl.Clock.GetProjectStatus() == BO.ProjectStatus.InProgress;
        NormalMode = 1;

        if (EngineerID != 0)
        {
            currentEngineer = s_bl.Engineer.Read(EngineerID);
            status = BO.Status.Scheduled;
        }
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for selection changed in the ComboBox
    /// </summary>
    private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Update the ListView based on filter changes
        UpdateListView();
    }

    /// <summary>
    /// Event handler for the "Add" button click
    /// </summary>
    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
        // Open TaskWindow for adding a new task
        new TaskWindow().ShowDialog();
        UpdateListView();
    }

    /// <summary>
    /// Event handler for double click on the ListView item
    /// </summary>
    private void UpdateListView_DoubleClick(object sender, RoutedEventArgs e)
    {
        // Get the selected task from the ListView
        BO.TaskInList? taskInList = (sender as ListView)?.SelectedItem as BO.TaskInList;
        if (taskInList != null)
        {
            // Handling based on recovery mode and engineer mode
            if (NormalMode != 0)
            {
                // Handling based on engineer and choose mission mode
                if (EngineerID == 0)
                {
                    // Open TaskWindow for updating the selected task
                    new TaskWindow(taskInList!.Id).ShowDialog();
                    UpdateListView();
                }
                else
                {
                    // Handling for engineer mode and choose mission mode
                    if (ChooseMission)
                    {
                        try
                        {
                            // Create a new task in engineer and update the task status to on track
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
            else
            {
                try
                {
                    // Recover the task in recovery mode
                    s_bl.Task.RecoverTask(taskInList.Id);

                    UpdateListView();

                    MessageBox.Show($"Task number {taskInList.Id} recovered successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch (BLDoesNotExistException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    /// <summary>
    /// Event handler for the "Recovery" button click
    /// </summary>
    private void BtnRecovery_Click(object sender, RoutedEventArgs e)
    {
        // Change the recovery mode
        NormalMode = NormalMode == 0 ? 1 : 0;
        UpdateListView();
    }

    /// <summary>
    /// Updates the ListView based on the selected experience level
    /// </summary>
    void UpdateListView()
    {
        // First, filter the list based on the selected status and after that, based on the selected experience level
        // and filter by recovery mode if needed
        if (NormalMode != 0)
        {
            if (EngineerID == 0)
            {
                // Normal mode, not in engineer mode
                TaskList = (status == BO.Status.None) ?
                    s_bl?.Task.ReadAll()!.OrderBy(item => item.Id)! :
                    s_bl?.Task.ReadAll(item => item.Status == status)!.OrderBy(item => item.Id)!;
            }
            else
            {
                // Normal mode, in engineer mode
                if (ChooseMission)
                {
                    // Engineer mode and choose mission mode
                    TaskList = (status == BO.Status.None) ?
                        s_bl?.Engineer.GetTasksOfEngineerToWork(EngineerID)!.OrderBy(item => item.Id)! :
                        s_bl?.Engineer.GetTasksOfEngineerToWork(EngineerID).Where(item => item.Status == status)!.OrderBy(item => item.Id)!;
                }
                else
                {
                    // Engineer mode and not choose mission mode
                    TaskList = (status == BO.Status.None) ?
                        s_bl?.Engineer.GetAllTaskOfEngineer(EngineerID).OrderBy(item => item.Id)! :
                        s_bl?.Engineer.GetAllTaskOfEngineer(EngineerID).Where(task => task.Status == status).OrderBy(item => item.Id)!;
                }
            }
        }
        else
        {
            // Recovery mode
            TaskList = (status == BO.Status.None) ?
                s_bl?.Task.GetDeletedTasks()!.OrderBy(item => item.Id)! :
                s_bl?.Task.GetDeletedTasks().Where(item => item.Status == status)!.OrderBy(item => item.Id)!;
        }

        // Filter by experience level
        TaskList = (complexity == BO.EngineerExperience.None) ?
            TaskList :
            TaskList.Where(item => s_bl!.Task.Read(item.Id).Complexity == complexity);
    }
}
