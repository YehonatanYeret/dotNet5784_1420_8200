namespace PL.Engineer;

using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window, INotifyPropertyChanged
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Flag to indicate whether to update or create a new engineer
    public bool UpdateOrCreate = false;

    // Event for property changed
    public event PropertyChangedEventHandler? PropertyChanged;
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
        set { SetValue(EngineerProperty, value); }
    }

    // Dependency property for CurrentEngineer
    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


    /// <summary>
    /// Gets or sets the image of the current engineer displayed in the window.
    /// </summary>
    public string? Image
    {
        get { return (string?)GetValue(ImageProperty); }
        set
        {
            SetValue(ImageProperty, value);
            OnPropertyChanged(nameof(Image)); // Notify the UI about the property change
        }
    }

    // Dependency property for image
    public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register("Image", typeof(string), typeof(EngineerWindow), new PropertyMetadata(null));


    public BO.User CurrentUser
    {
        get { return (BO.User)GetValue(UserProperty); }
        set { SetValue(UserProperty, value); }
    }

    public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(EngineerWindow), new PropertyMetadata(null));

    /// <summary>
    /// Constructor for EngineerWindow
    /// </summary>
    /// <param name="id">Engineer ID</param>
    public EngineerWindow(int id = 0)
    {
        InitializeComponent();
        UpdateOrCreate = (id == 0);
        CurrentEngineer = UpdateOrCreate ? new BO.Engineer() : s_bl.Engineer.Read(id);
        // Set the image of the engineer
        Image = CurrentEngineer.Image;
        CurrentUser = UpdateOrCreate? new BO.User() : s_bl.User.Read(CurrentEngineer.Email);
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for the Add/Update button click
    /// </summary>
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        // Set the image of the engineer
        CurrentEngineer.Image = Image;
        try
        {
            BO.User user = new BO.User()
            {
                Type = BO.UserType.engineer,
                Name = CurrentEngineer.Name,
                Password = CurrentUser.Password,
                Email = CurrentEngineer.Email
            };

            // Call the appropriate method in the business logic layer based on the update or create flag
            if (UpdateOrCreate)
            {
                s_bl.User.Create(user);

                s_bl.Engineer.Create(CurrentEngineer);

                MessageBox.Show("the the engineer created successfully", "operation succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                s_bl.User.Update(user);

                s_bl.Engineer.Update(CurrentEngineer);
                MessageBox.Show("the the engineer updated successfully", "operation succeed",
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
    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (MessageBox.Show("Are you sure you want to delete this engineer?", "Delete Engineer", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                s_bl.User.Delete(CurrentEngineer.Email);
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

    private void PasswordBox_PasswordChange(object sender, RoutedEventArgs e)
    {
        CurrentUser.Password = ((PasswordBox)sender).Password;
    }


    private void btnBrowse_Click(object sender, RoutedEventArgs e)
    {
        // Open file dialog to select an image file and check for valid image file
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp";

        // If the user selects a file, convert the image to base64 and set the image property
        if (fileDialog.ShowDialog() == true)
        {
            string filePath = fileDialog.FileName;
            try
            {
                Image = s_bl.Engineer.ConvertImageToBase64(filePath);
            }
            // If the file is not a valid image file, show an error message
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
