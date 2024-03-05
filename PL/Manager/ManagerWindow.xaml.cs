namespace PL.Manager;

using PL.Task;
using System.ComponentModel;
using System.Windows;


/// <summary>
/// Interaction logic for ManagerWindow.xaml
/// </summary>
public partial class ManagerWindow : Window, INotifyPropertyChanged
{
    // Dependency property to handle visibility of project start indication
    public Visibility IsprojectStarted
    {
        get { return (Visibility)GetValue(IsprojectStartedProperty); }
        set
        {
            SetValue(IsprojectStartedProperty, value);
            OnPropertyChanged(nameof(IsprojectStarted)); // Notify the UI about the property change
        }
    }

    // Dependency property for IsprojectStarted
    public static readonly DependencyProperty IsprojectStartedProperty =
        DependencyProperty.Register("IsprojectStarted", typeof(Visibility), typeof(ManagerWindow), new PropertyMetadata(null));

    // Dependency property to handle percentage of completed tasks
    public double percentComplete
    {
        get { return (double)GetValue(percentCompleteProperty); }
        set
        {
            SetValue(percentCompleteProperty, value);
            OnPropertyChanged(nameof(percentComplete)); // Notify the UI about the property change
        }
    }

    // Dependency property for percentComplete
    public static readonly DependencyProperty percentCompleteProperty =
        DependencyProperty.Register("percentComplete", typeof(double), typeof(ManagerWindow), new PropertyMetadata(null));

    // Business logic layer instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Email { get; set; }
    public string ManagerName { get; set; }

    // Constructor for ManagerWindow
    public ManagerWindow(string email)
    {
        Email = email;
        ManagerName = s_bl.User.Read(email).Name;


        Activated += ManagerWindow_Activated;

        // Set visibility based on project status
        IsprojectStarted = (s_bl.Clock.GetProjectStatus() == BO.ProjectStatus.InProgress) ? Visibility.Hidden : Visibility.Visible;

        InitializeComponent();
    }

    private void ManagerWindow_Activated(object? sender, EventArgs e)
    {
        // Calculate percentage of completed tasks
        int allTasks = s_bl.Task.ReadAll().Count();
        int completedTasks = s_bl.Task.ReadAll().Where(t => t.Status == BO.Status.Done).Count();
        percentComplete = ((double)completedTasks / allTasks) * 100;
    }

    // Event handler for EngineerButton click
    private void EngineerButton_Click(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerListWindow().Show();
    }

    // Event handler for TaskButton click
    private void TaskButton_Click(object sender, RoutedEventArgs e)
    {
        new TaskListWindow().ShowDialog();
    }

    // Event handler for ResetDataBtn click
    private void ResetDataBtn_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to reset all the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
        {
            // Reset the database
            s_bl.ResetDB();
            IsprojectStarted = Visibility.Visible;
            s_bl.Init();
        }
    }

    // Event handler for InitializeDataBtn click
    private void InitializeDataBtn_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to initialize the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
        {
            // Initialize the database
            s_bl.InitializeDB();
            IsprojectStarted = Visibility.Visible;
            s_bl.Init();
            MessageBox.Show("Data initialized successfully");
        }
    }

    // Event handler for GanttClick_Button click
    private void GanttClick_Button(object sender, RoutedEventArgs e)
    {
        new GantWindow().Show();
    }

    // Event handler for StartProject_Click click
    private void StartProject_Click(object sender, RoutedEventArgs e)
    {
        // Ask for the project start date using a custom dialog box
        var dialogBox = new Messeges.PickDate();

        // If the user chose a date, set all project dates
        if (dialogBox.ShowDialog() == true)
        {
            DateTime? startProject = (DateTime?)dialogBox.Date;

            // Start the project
            if (startProject is not null)
            {
                DateTime start = (DateTime)startProject;
                s_bl.Task.StartProject(start);

                // Update visibility based on project status
                if (s_bl.Clock.GetProjectStatus() == BO.ProjectStatus.InProgress)
                {
                    IsprojectStarted = Visibility.Hidden;
                }
            }
        }
    }

    // Invoke property changed event
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Event handler for BtnCreateManager_Click click
    private void BtnCreateManager_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            //for checking self changing
            BO.User manager = s_bl.User.Read(Email);

            // Open the CreateManagerWindow for creating a new manager
            new CreateManagerWindow(0, Email, Email).ShowDialog();
            if (ManagerName != manager.Name || s_bl.User.Read(Email).Password != manager.Password)
                Close();
        }
        catch
        {
            //catch when self Email change
            Close();
        }
    }
}
