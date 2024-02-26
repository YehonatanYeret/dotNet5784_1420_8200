namespace PL.Task;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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

    public List<DependencyList> Dep { get { return (List<DependencyList>)GetValue(DepProperty); } set { SetValue(DepProperty, value); }}

    // Identifies the CurrentTask dependency property.
    public static readonly DependencyProperty DepProperty =
        DependencyProperty.Register("Dep", typeof(List<DependencyList>), typeof(TaskWindow), new PropertyMetadata(null));


    /// <summary>
    /// Initializes a new instance of the TaskWindow class.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    public TaskWindow(int id = 0)
    {

        UpdateOrCreate = (id == 0);
        CurrentTask = UpdateOrCreate ? new BO.Task() : s_bl.Task.Read(id);
        CurrEngineer = (CurrentTask.Engineer != null) ? CurrentTask.Engineer : null;


        Dep = (from t in s_bl.Task.ReadAll()
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
            new Engineer.EngineerListWindow(CurrentTask.Id).ShowDialog();
            CurrEngineer = (s_bl.Task.Read(CurrentTask.Id).Engineer != null) ? s_bl.Task.Read(CurrentTask.Id).Engineer : null;
        }
        else
        {
            // Show the EngineerWindow for the selected engineer.
            new Engineer.EngineerWindow(CurrEngineer.Id).ShowDialog();
            CurrEngineer = (s_bl.Task.Read(CurrentTask.Id).Engineer != null) ? s_bl.Task.Read(CurrentTask.Id).Engineer : null;
        }
    }

    private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CurrentTask.Engineer = CurrEngineer;

            CurrentTask.Dependencies = (from t in Dep
                                        where t.IsDep
                                        select t.Task).ToList();

            // Call the appropriate method in the business logic layer based on the update or create flag
            if (UpdateOrCreate)
            {
                s_bl.Task.Create(CurrentTask);
                MessageBox.Show("the the task created successfully", "operation succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                s_bl.Task.Update(CurrentTask);
                MessageBox.Show("the the task updated successfully", "operation succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            // Close the window after successful operation
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

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (MessageBox.Show("Are you sure you want to delete this task?", "Delete Task", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                s_bl.Task.Delete(CurrentTask.Id);
                MessageBox.Show("Task Deleted successfully", "Delete Engineer",
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
}

/// <summary>
/// class to represent the dependency list with check box.
/// </summary>
public class DependencyList
{
    public BO.TaskInList Task { get; set; }
    public bool IsDep { get; set; }
}