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

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        new Manager.ManagerWindow().Show();
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        var dialogBox = new Messeges.EnterIdMessege("Enter engineer Id to show:");
        if (dialogBox.ShowDialog() == true)
        {
            try
            {
                if (!int.TryParse(dialogBox.Answer, out int id))
                    throw new BO.BLValueIsNotCorrectException("You didn't entered proper Id");
                BO.Engineer engineer = s_bl.Engineer.Read(id);
                new Engineer.EngineerShowWindow(engineer.Id).Show();
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
