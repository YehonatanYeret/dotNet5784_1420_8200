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

    // Dependency property for the Task.
    public BO.Task CurrentTask { get { return (BO.Task)GetValue(TaskProperty); } set { SetValue(TaskProperty, value); } }

    // Identifies the CurrentTask dependency property.
    public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

    public BO.EngineerInTask? CurrEngineer { get { return (BO.EngineerInTask?)GetValue(CurrEngineerProperty); } set { SetValue(CurrEngineerProperty, value); } }

    // Identifies the CurrentTask dependency property.
    public static readonly DependencyProperty CurrEngineerProperty =
        DependencyProperty.Register("CurrEngineer", typeof(BO.EngineerInTask), typeof(TaskWindow), new PropertyMetadata(null));

    public List<DependencyList> Dep { get { return (List<DependencyList>)GetValue(DepProperty); } set { SetValue(DepProperty, value); } }

    // Identifies the CurrentTask dependency property.
    public static readonly DependencyProperty DepProperty =
        DependencyProperty.Register("Dep", typeof(List<DependencyList>), typeof(TaskWindow), new PropertyMetadata(null));

    public bool IsProjectStarted
    {
        get { return (bool)GetValue(IsProjectStartedProperty); }
        set { SetValue(IsProjectStartedProperty, value); }
    }

    // Dependency property for indicating project status.
    public static readonly DependencyProperty IsProjectStartedProperty =
        DependencyProperty.Register("IsProjectStartedBool", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Initializes a new instance of the TaskWindow class.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    public TaskWindow(int id = 0)
    {
        // Determine whether to update or create a new task.
        UpdateOrCreate = (id == 0);
        // Initialize CurrentTask based on ID.
        CurrentTask = UpdateOrCreate ? new BO.Task() : s_bl.Task.Read(id);

        // Determine if the project is in progress.
        IsProjectStarted = s_bl.Clock.GetProjectStatus() != BO.ProjectStatus.InProgress;

        // Set the current engineer for the task.
        CurrEngineer = CurrentTask.Engineer;

        // Initialize the list of dependencies for the task.
        Dep = (from t in s_bl.Task.ReadAll(task => task.Id != id)
               select new DependencyList()
               {
                   Task = t,
                   IsDep = UpdateOrCreate ? false : CurrentTask.Dependencies!.Any(x => x.Id == t.Id)
               }).ToList();

        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event of the Engineer in Task button.
    /// </summary>
    private void EngeineerInTaskButton_Click(object sender, RoutedEventArgs e)
    {
        if (CurrEngineer == null)
        {
            // Show the EngineerListWindow to select an engineer.
            Engineer.EngineerListWindow engineerlistWiindow = new Engineer.EngineerListWindow(CurrentTask.Id, CurrentTask.Complexity);
            engineerlistWiindow.ShowDialog();
            CurrEngineer = engineerlistWiindow.choosenEngineer;
        }
        else
        {
            // Show the EngineerWindow for the selected engineer.
            new Engineer.EngineerWindow(CurrEngineer.Id).ShowDialog();
            try
            {
                // Update the current engineer after modification.
                CurrEngineer = s_bl.Engineer.GetEngineerInTask(CurrEngineer.Id);
            }
            catch
            {
                // If an exception occurs, set the current engineer to null.
                CurrEngineer = null;
            }
        }
    }

    /// <summary>
    /// Handles the click event of the Add/Update button.
    /// </summary>
    private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Set the engineer of the task to the current engineer.
            CurrentTask.Engineer = CurrEngineer;

            CurrentTask.Dependencies = (from t in Dep
                                        where t.IsDep
                                        select t.Task).ToList();

            // Call the appropriate method in the business logic layer based on the update or create flag
            if (UpdateOrCreate)
            {
                s_bl.Task.Create(CurrentTask);
                MessageBox.Show("The task created successfully", "Operation Succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                s_bl.Task.Update(CurrentTask);
                MessageBox.Show("The task updated successfully", "Operation Succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            // Close the window after a successful operation
            Close();
        }
        catch (BO.BLAlreadyExistsException ex)
        {
            MessageBox.Show(ex.Message, "Error occurred while creation",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (BO.BLDoesNotExistException ex)
        {
            MessageBox.Show(ex.Message, "Error occurred while updation",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Unknown error occurred",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Handles the click event of the Delete button.
    /// </summary>
    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (MessageBox.Show("Are you sure you want to delete this task?", "Delete Task", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                s_bl.Task.Delete(CurrentTask.Id);
                MessageBox.Show("Task deleted successfully", "Delete Task",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        catch (BO.BLDoesNotExistException ex)
        {
            MessageBox.Show(ex.Message, "Error occurred while deletion",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Unknown error occurred",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            Close();
        }
    }

    /// <summary>
    /// Handles the selection change event of the ComboBox.
    /// </summary>
    private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        // Check if the selected engineer is qualified for the task.
        if (CurrEngineer != null && s_bl.Engineer.Read(CurrEngineer.Id).Level < CurrentTask.Complexity)
        {
            MessageBox.Show("The engineer is not qualified for this task", "Error occurred while updation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            CurrEngineer = null;
        }
    }
}

/// <summary>
/// Class to represent the dependency list with a checkbox.
/// </summary>
public class DependencyList
{
    public BO.TaskInList Task { get; set; }
    public bool IsDep { get; set; }
}
