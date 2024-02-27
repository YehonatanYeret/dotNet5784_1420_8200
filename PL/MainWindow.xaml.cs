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

    public BO.User CurrentUser
    {
        get { return (BO.User)GetValue(userProperty); }
        set { SetValue(userProperty, value); }
    }

    public static readonly DependencyProperty userProperty =
        DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));

    /// <summary>
    /// Constructor for the MainWindow class.
    /// Initializes the main window and its components.
    /// </summary>
    public MainWindow()
    {
        CurrentUser = new BO.User();
        InitializeComponent();
    }

    private void BtnEnterUser_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            BO.User user = s_bl.User.Read(CurrentUser.Email);

            if (user.Password == CurrentUser.Password)
            {
                MessageBox.Show("Welcome " + user.Name, "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);
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
        catch(BO.BLDoesNotExistException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
  
    }

    private void BtnCreateManager_Click(object sender, RoutedEventArgs e)
    {
        new Manager.CreateManagerWindow().ShowDialog();
    }
    private void PasswordBox_PasswordChange(object sender, RoutedEventArgs e)
    {
        CurrentUser.Password = ((PasswordBox)sender).Password;
    }
}
