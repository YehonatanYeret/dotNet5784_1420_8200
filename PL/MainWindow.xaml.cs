namespace PL;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Dependency property for the current user
    public BO.User CurrentUser
    {
        get { return (BO.User)GetValue(userProperty); }
        set { SetValue(userProperty, value); }
    }

    // Identifies the CurrentUser dependency property
    public static readonly DependencyProperty userProperty =
        DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));

    // Dependency property for the current date
    public DateTime Date
    {
        get { return (DateTime)GetValue(dateProperty); }
        set { SetValue(dateProperty, value); }
    }

    // Identifies the Date dependency property
    public static readonly DependencyProperty dateProperty =
        DependencyProperty.Register("Date", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now));

    /// <summary>
    /// Constructor for the MainWindow class.
    /// Initializes the main window and its components.
    /// </summary>
    public MainWindow()
    {
        // Set the initial date and create a new user
        Date = s_bl.Time;
        CurrentUser = new BO.User();
        if (s_bl.User.ReadAll().Count() == 0)
            s_bl.User.Create(new BO.User
            {
                Type = BO.UserType.manager,
                Name = "admin",
                Password = "Admin123",
                Email = "admin@gmail.com"
            });
        InitializeComponent();
    }

    // Event handler for the "Enter User" button click
    private void BtnEnterUser_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Read the user from the business logic layer
            BO.User user = s_bl.User.Read(CurrentUser.Email);

            // Check if the entered password matches the user's password
            if (user.Password == CurrentUser.Password)
            {
                MessageBox.Show("Welcome " + user.Name, "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);

                // Open different windows based on user type
                if (user.Type == BO.UserType.engineer)
                {
                    BO.Engineer engineer = s_bl.Engineer.ReadAll(engineer => engineer.Email == user.Email).FirstOrDefault()!;
                    new Engineer.EngineerShowWindow(engineer.Id).Show();
                }
                else
                {
                    new Manager.ManagerWindow(user.Email).Show();
                }
            }
            else
            {
                MessageBox.Show("Incorrect password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (BO.BLDoesNotExistException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Event handler for the "Create Manager" button click
    private void BtnCreateManager_Click(object sender, RoutedEventArgs e)
    {
        new Manager.CreateManagerWindow().ShowDialog();
    }

    // Event handler for the password change event
    private void PasswordBox_PasswordChange(object sender, RoutedEventArgs e)
    {
        // Set the password property when the password changes
        CurrentUser.Password = ((PasswordBox)sender).Password;
    }

    // Event handler for the "Add Hour" button click
    private void BtnHour_Click(object sender, RoutedEventArgs e)
    {
        // Add an hour in the business logic layer and update the date
        s_bl.AddHour();
        Date = s_bl.Time;
    }

    // Event handler for the "Add Day" button click
    private void BtnDay_Click(object sender, RoutedEventArgs e)
    {
        // Add a day in the business logic layer and update the date
        s_bl.AddDay();
        Date = s_bl.Time;
    }
}
