using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Constructor for the MainWindow class.
    /// Initializes the main window and its components.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for the "Initialize DataBase" button click event.
    /// Shows a message box asking the user if they want to initialize the data.
    /// If the user confirms, initializes the data using DalTest.Initialization.Do().
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event data.</param>

    //private void Initialization_click(object sender, RoutedEventArgs e)
    //{
    //    if (MessageBoxResult.Yes == MessageBox.Show("Do you want to initialization the data?", "Initialization", MessageBoxButton.YesNo))
    //    {
    //        DalTest.Initialization.Do();
    //    }
    //}

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        new Manager.ManagerWindow().Show();
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        var dialogBox = new Messeges.EnterIdMessege();
        if (dialogBox.ShowDialog() == true)
        {
            try
            {
                if (!int.TryParse(dialogBox.AnswerId, out int id))
                    throw new BO.BLValueIsNotCorrectException("You didn't entered proper Id");
                BO.Engineer engineer = s_bl.Engineer.Read(id);
                 new Engineer.ShowEngineer(engineer).Show();
            }
            catch
            {
                MessageBox.Show("You didnt enterd a proper id", "Unknown error occurred",
    MessageBoxButton.OK,
    MessageBoxImage.Error);
            }
        }
    }
}
