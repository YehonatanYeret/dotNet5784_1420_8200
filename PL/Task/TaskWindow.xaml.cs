namespace PL.Task;

using System.Windows;

/// <summary>
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Flag to indicate whether to update or create a new task
    public bool UpdateOrCreate = false;

    /// <summary>
    /// Dependency Property for Task
    /// </summary>
    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(TaskProperty); }
        set { SetValue(TaskProperty, value); }
    }

    public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

    /// <summary>
    /// Constructor for EngineerWindow
    /// </summary>
    /// <param name="id">Engineer ID</param>
    public TaskWindow(int id = -1)
    {
        InitializeComponent();
        UpdateOrCreate = (id == -1);
        CurrentTask = UpdateOrCreate ? new BO.Task() : s_bl.Task.Read(id);
    }

    /// <summary>
    /// Event handler for the Add/Update button click
    /// </summary>
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Call the appropriate method in the business logic layer based on the update or create flag
            if (UpdateOrCreate)
                s_bl.Task.Create(CurrentTask);
            else
                s_bl.Task.Update(CurrentTask);

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

    private void EngeineerInTaskButton_Click(object sender, RoutedEventArgs e)
    {
        if(CurrentTask.Engineer == null)
        {
            new Engineer.EngineerListWindow(CurrentTask.Id).ShowDialog();
        }
        else
        {
            new Engineer.ShowEngineer(CurrentTask.Engineer.Id).ShowDialog();
        }
    }
}
