namespace PL.Manager;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;


/// <summary>
/// Interaction logic for CreateManagerWindow.xaml
/// </summary>
public partial class CreateManagerWindow : Window ,INotifyPropertyChanged
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.User CurrentManager
    {
        get { return (BO.User)GetValue(userProperty); }
        set { SetValue(userProperty, value);
            OnPropertyChanged(nameof(CurrentManager));
        }
    }

    public static readonly DependencyProperty userProperty =
        DependencyProperty.Register("CurrentManager", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));

    // Event for property changed
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string? Email { get; set; }
    public string? ManagerEmail { get; set; }
    public int createOrUpdate
    {
        get { return (int)GetValue(createOrUpdateProperty); }
        set { SetValue(createOrUpdateProperty, value);
            OnPropertyChanged(nameof(createOrUpdate));
        }
    }

        public static readonly DependencyProperty createOrUpdateProperty =
        DependencyProperty.Register("createOrUpdate", typeof(int), typeof(CreateManagerWindow), new PropertyMetadata(null));
    public int Place { get; set; }

    public CreateManagerWindow(int place = 0, string? managerEmail = "", string? email = "")
    {
        Email = email;
        Place = place;
        ManagerEmail = managerEmail;
        createOrUpdate = string.IsNullOrEmpty(email) ? 0 : 1;
        CurrentManager = (createOrUpdate == 0) ? new BO.User() : s_bl.User.Read(email!);
        InitializeComponent();
    }

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (createOrUpdate == 0)
            {
                CurrentManager.Type = BO.UserType.manager;
                s_bl.User.Create(CurrentManager);

                MessageBox.Show("the the manager created successfully", "operation succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Close();
            }
            else
            {
                s_bl.User.Update(CurrentManager);
                MessageBox.Show("the the manager updated successfully", "operation succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                Close();
            }
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

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (ManagerEmail == Email)
        {
            MessageBox.Show("You can't delete yourself", "Error occurred while deletion",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Close();
        }
        else
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to delete this manager?", "Delete Manager", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    s_bl.User.Delete(CurrentManager.Email);
                    MessageBox.Show("Engineer Deleted successfully", "Delete Engineer",
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

    private void PasswordBox_PasswordChange(object sender, RoutedEventArgs e)
    {
        CurrentManager.Password = ((PasswordBox)sender).Password;
    }

    private void btnLeft_Click(object sender, RoutedEventArgs e)
    {
        Place--;
        if (Place < 0)
            Place++;

        CurrentManager = s_bl.User.ReadAll(u => u.Type == BO.UserType.manager).ToList().ElementAt(Place);
    }

    private void btnRight_Click(object sender, RoutedEventArgs e)
    {
        Place++;
        if(Place > s_bl.User.ReadAll(u => u.Type == BO.UserType.manager).Count() - 1)
            Place--;

        CurrentManager = s_bl.User.ReadAll(u => u.Type == BO.UserType.manager).ToList().ElementAt(Place);
    }

    private void btnNew_Click(object sender, RoutedEventArgs e)
    {
        CurrentManager = new BO.User();
        createOrUpdate = 0;
    }
}
