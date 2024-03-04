namespace PL.Engineer;

using PL.Task;
using System.ComponentModel;
using System.Linq;
using System.Windows;

/// <summary>
/// Interaction logic for EngineerShowWindow.xaml
/// </summary>
public partial class EngineerShowWindow : Window, INotifyPropertyChanged
{
    // Static instance of the business logic
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Event for property changed
    public event PropertyChangedEventHandler? PropertyChanged;

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

    // Dependency property for the Task.
    public BO.Task? CurrTask { get { return (BO.Task)GetValue(TaskProperty); } set { SetValue(TaskProperty, value); OnPropertyChanged(nameof(CurrTask)); } }

    // Identifies the CurrentTask dependency property.
    public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));



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
            CurrTask = s_bl.Task.Read(CurrentEngineer.Task.Id);
        else
            CurrTask = new BO.Task() { Id = 0 };

        InitializeComponent();
    }

    /// <summary>
    /// Event handler for choosing a task.
    /// </summary>
    private void ChooseTaskBtn_Click(object sender, RoutedEventArgs e)
    {

        if (!s_bl.Engineer.GetTasksOfEngineerToWork(CurrentEngineer.Id).Any())
        {
            MessageBox.Show("There are no tasks to choose from", "No tasks", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Open a window for selecting tasks
        new TaskListWindow(CurrentEngineer.Id).ShowDialog();

        CurrentEngineer = s_bl.Engineer.Read(CurrentEngineer.Id);
        if (CurrentEngineer.Task != null)
            CurrTask = s_bl.Task.Read(CurrentEngineer.Task.Id);
        else
            CurrTask = new BO.Task() { Id = 0 };
    }

    /// <summary>
    /// Event handler for finishing a task.
    /// </summary>
    private void FinishTaskBtn_Click(object sender, RoutedEventArgs e)
    {
        s_bl.Task.Update(CurrTask!);

        // Change the status of the current task
        s_bl.Task.ChangeStatusOfTask(CurrTask!.Id);

        // Reset the current task to a default value
        CurrTask = new BO.Task() { Id = 0 };

    }

    private void EngineerAllTasksBtn_Click(object sender, RoutedEventArgs e)
    {
        // Open a window for displaying all tasks of the engineer but in read-only mode
        new TaskListWindow(CurrentEngineer.Id, false).ShowDialog();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Task.Update(CurrTask!);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Task.Update(CurrTask!);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
