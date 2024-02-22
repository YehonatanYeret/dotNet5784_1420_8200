namespace PL.Engineer;

using PL.Task;
using System.ComponentModel;
using System.Windows;

/// <summary>
/// Interaction logic for EngineerShowWindow.xaml
/// </summary>
public partial class EngineerShowWindow : Window, INotifyPropertyChanged
{
    // Static instance of the business logic
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Event for property changed
    public event PropertyChangedEventHandler PropertyChanged;

    // Invoke property changed event
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Gets or sets the current engineer displayed in the window.
    /// </summary>
    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set
        {
            SetValue(EngineerProperty, value);
            OnPropertyChanged(nameof(CurrentEngineer)); // Notify the UI about the property change
        }
    }

    // Dependency property for CurrentEngineer
    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerShowWindow), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the current task associated with the engineer.
    /// </summary>
    public BO.TaskInList Task
    {
        get { return task; }
        set
        {
            task = value;
            OnPropertyChanged(nameof(Task)); // Notify the UI about the property change
        }
    }
    private BO.TaskInList task;

    /// <summary>
    /// Initializes a new instance of the <see cref="EngineerShowWindow"/> class.
    /// </summary>
    /// <param name="id">The ID of the engineer to display.</param>
    public EngineerShowWindow(int id)
    {
        // Read the engineer information from the business logic
        CurrentEngineer = s_bl.Engineer.Read(id);

        // Set the associated task or create a default one
        if (CurrentEngineer.Task != null)
            Task = s_bl.Task.ConvertToTaskInList(CurrentEngineer.Task.Id);
        else
            Task = new BO.TaskInList() { Id = 0 };

        InitializeComponent();
    }

    /// <summary>
    /// Event handler for choosing a task.
    /// </summary>
    private void ChooseTaskBtn_Click(object sender, RoutedEventArgs e)
    {
        // Open a window for selecting tasks
        TaskListWindow taskListWindow = new TaskListWindow();
        taskListWindow.Owner = this;
        if (taskListWindow.setupWindow(CurrentEngineer!.Id))
            taskListWindow.ShowDialog();

        // Update the current engineer and associated task after selecting a task
        CurrentEngineer = s_bl.Engineer.Read(CurrentEngineer.Id);
        if (CurrentEngineer.Task != null)
            Task = s_bl.Task.ConvertToTaskInList(CurrentEngineer.Task.Id);
        else
            Task = new BO.TaskInList() { Id = 0 };
    }

    /// <summary>
    /// Event handler for finishing a task.
    /// </summary>
    private void FinishTaskBtn_Click(object sender, RoutedEventArgs e)
    {
        // Change the status of the current task
        s_bl.Task.ChangeStatusOfTask(Task!.Id);

        // Reset the current task to a default value
        Task = new BO.TaskInList() { Id = 0 };
    }
}
