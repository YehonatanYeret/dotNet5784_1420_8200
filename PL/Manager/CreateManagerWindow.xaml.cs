namespace PL.Manager;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for CreateManagerWindow.xaml
/// </summary>
public partial class CreateManagerWindow : Window, INotifyPropertyChanged
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Property for the current manager
    public BO.User CurrentManager
    {
        get { return (BO.User)GetValue(userProperty); }
        set
        {
            SetValue(userProperty, value);
            OnPropertyChanged(nameof(CurrentManager));
        }
    }

    // Dependency property for the current manager
    public static readonly DependencyProperty userProperty =
        DependencyProperty.Register("CurrentManager", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));

    // Event for property changed
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Properties for email and manager email
    public string? Email { get; set; }
    public string? ManagerEmail { get; set; }

    // Property for create or update flag
    public int createOrUpdate
    {
        get { return (int)GetValue(createOrUpdateProperty); }
        set
        {
            SetValue(createOrUpdateProperty, value);
            OnPropertyChanged(nameof(createOrUpdate));
        }
    }

    // Dependency property for create or update flag
    public static readonly DependencyProperty createOrUpdateProperty =
        DependencyProperty.Register("createOrUpdate", typeof(int), typeof(CreateManagerWindow), new PropertyMetadata(null));

    // Property for the place
    public int Place { get; set; }

    // Constructor
    public CreateManagerWindow(int place = 0, string? managerEmail = "", string? email = "")
    {
        // Initialize properties with provided values
        Email = email;
        Place = place;
        ManagerEmail = managerEmail;
        createOrUpdate = string.IsNullOrEmpty(email) ? 0 : 1;
        CurrentManager = (createOrUpdate == 0) ? new BO.User() : s_bl.User.Read(email!);

        // Initialize the window components
        InitializeComponent();
    }

    // Event handler for add/update button click
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (createOrUpdate == 0)
            {
                // Create a new manager
                CurrentManager.Type = BO.UserType.manager;
                s_bl.User.Create(CurrentManager);

                // Display success message
                MessageBox.Show("The manager created successfully", "Operation Succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Close the window
                Close();
            }
            else
            {
                if(ManagerEmail == Email)
                {
                    if (MessageBox.Show("Change yourself data will cause to log out\nAre you sure about that?", "Self change data",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information) == MessageBoxResult.No)
                        return;
                }
                // Delete the old manager and create a new one (because the email is the primary key)
                s_bl.User.Delete(Email!);
                s_bl.User.Create(CurrentManager);

                // Display success message
                MessageBox.Show("The manager updated successfully", "Operation Succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Close the window
                Close();
            }
        }
        catch (BO.BLAlreadyExistsException ex)
        {
            // Display error message for duplicate manager
            MessageBox.Show(ex.Message, "Error occurred while creation",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (BO.BLDoesNotExistException)
        {
            // Handle a specific exception
            s_bl.User.Create(CurrentManager);
        }
        catch (Exception ex)
        {
            // Display a generic error message for any other exceptions
            MessageBox.Show(ex.Message, "Unknown error occurred",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    // Event handler for delete button click
    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (ManagerEmail == Email)
        {
            // Display error message for attempting to delete oneself
            MessageBox.Show("You can't delete yourself", "Error occurred while deletion",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            // Close the window
            Close();
        }
        else
        {
            try
            {
                // Confirm manager deletion with a message box
                if (MessageBox.Show("Are you sure you want to delete this manager?", "Delete Manager", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Delete the manager
                    s_bl.User.Delete(CurrentManager.Email);

                    // Display success message
                    MessageBox.Show("Manager Deleted successfully", "Delete Manager",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (BO.BLDoesNotExistException ex)
            {
                // Display error message for non-existent manager
                MessageBox.Show(ex.Message, "Error occurred while deletion",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Display a generic error message for any other exceptions
                MessageBox.Show(ex.Message, "Unknown error occurred",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                // Close the window
                Close();
            }
        }
    }

    // Event handler for password box change
    private void PasswordBox_PasswordChange(object sender, RoutedEventArgs e)
    {
        // Set the password of the manager to the password box value only if it's not empty
        string pass = ((PasswordBox)sender).Password;

        if (!string.IsNullOrEmpty(pass))
            CurrentManager.Password = pass;
    }

    // Event handler for left navigation button click
    private void btnLeft_Click(object sender, RoutedEventArgs e)
    {
        // Navigate to the previous manager
        Place--;
        if (Place < 0)
            Place++;

        CurrentManager = s_bl.User.ReadAll(u => u.Type == BO.UserType.manager).ToList().ElementAt(Place);
        Email = CurrentManager.Email;
    }

    // Event handler for right navigation button click
    private void btnRight_Click(object sender, RoutedEventArgs e)
    {
        // Navigate to the next manager
        Place++;
        if (Place > s_bl.User.ReadAll(u => u.Type == BO.UserType.manager).Count() - 1)
            Place--;

        CurrentManager = s_bl.User.ReadAll(u => u.Type == BO.UserType.manager).ToList().ElementAt(Place);
        Email = CurrentManager.Email;
    }

    // Event handler for new button click
    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
        // Create a new manager and set the mode to create
        CurrentManager = new BO.User();
        createOrUpdate = 0;
        Email = null;
    }
}
