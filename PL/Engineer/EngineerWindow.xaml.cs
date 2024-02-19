namespace PL.Engineer;

using System.Windows;

/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Flag to indicate whether to update or create a new engineer
    public bool UpdateOrCreate = false;

    /// <summary>
    /// Dependency Property for Engineer
    /// </summary>
    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

    /// <summary>
    /// Constructor for EngineerWindow
    /// </summary>
    /// <param name="id">Engineer ID</param>
    public EngineerWindow(int id = 0)
    {
        InitializeComponent();
        UpdateOrCreate = id == 0;
        CurrentEngineer = UpdateOrCreate ? new BO.Engineer() : s_bl.Engineer.Read(id);
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
            {
                s_bl.Engineer.Create(CurrentEngineer);
                MessageBox.Show("the the engineer created successfully", "operation succeed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
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
                s_bl.Engineer.Delete(CurrentEngineer.Id);
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
