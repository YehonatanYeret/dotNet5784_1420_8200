namespace PL.Task;

using System.ComponentModel;
using System.Windows;


/// <summary>
/// Represents the Task Window for managing tasks.
/// </summary>
public partial class TaskWindow : Window, INotifyPropertyChanged
{
    // Get the instance of the business logic.
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Indicates whether to update or create a new task.
    // True means create and false means update.
    public bool UpdateOrCreate = false;

    // Event raised when a property value changes.
    public event PropertyChangedEventHandler? PropertyChanged;

    // Invokes the property changed event.
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Dependency property for the Task.
    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(TaskProperty); }
        set { SetValue(TaskProperty, value); }
    }

    // Identifies the CurrentTask dependency property.
    public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

    // Gets or sets the Engineer associated with the task.
    public BO.EngineerInTask? Engineer
    {
        get { return engineer; }
        set
        {
            engineer = value;
            OnPropertyChanged(nameof(Engineer));
        }
    }
    private BO.EngineerInTask? engineer;

    /// <summary>
    /// Initializes a new instance of the TaskWindow class.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    public TaskWindow(int id = 0)
    {
        UpdateOrCreate = (id == 0);
        CurrentTask = UpdateOrCreate ? new BO.Task() : s_bl.Task.Read(id);
        Engineer = (CurrentTask.Engineer != null) ? CurrentTask.Engineer : null;
        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event of the Add/Update button.
    /// </summary>
    private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Call the appropriate method in the business logic layer based on the update or create flag.
            if (UpdateOrCreate)
                s_bl.Task.Create(CurrentTask);
            else
                s_bl.Task.Update(CurrentTask);

            // Close the window after successful operation.
            Close();
        }
        catch (BO.BLAlreadyExistsException ex)
        {
            MessageBox.Show(ex.Message, "Error occurred while creation", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (BO.BLDoesNotExistException ex)
        {
            MessageBox.Show(ex.Message, "Error occurred while updation", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Unknown error occurred", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Handles the click event of the Engineer in Task button.
    /// </summary>
    private void EngeineerInTaskButton_Click(object sender, RoutedEventArgs e)
    {
        if (Engineer == null)
        {
            // Show the EngineerListWindow to select an engineer.
            new Engineer.EngineerListWindow(CurrentTask.Id).ShowDialog();
            Engineer = (s_bl.Task.Read(CurrentTask.Id).Engineer != null) ? s_bl.Task.Read(CurrentTask.Id).Engineer : null;
        }
        else
        {
            // Show the EngineerWindow for the selected engineer.
            new Engineer.EngineerWindow(Engineer.Id).ShowDialog();
            Engineer = (s_bl.Task.Read(CurrentTask.Id).Engineer != null) ? s_bl.Task.Read(CurrentTask.Id).Engineer : null;
        }
    }
}
