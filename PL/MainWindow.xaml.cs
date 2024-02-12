using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Constructor for the MainWindow class.
    /// Initializes the main window and its components.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for the "Handle Engineers" button click event.
    /// Opens the EngineerListWindow.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event data.</param>
    private void EngineerList_click(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerListWindow().Show();
    }

    /// <summary>
    /// Event handler for the "Initialize DataBase" button click event.
    /// Shows a message box asking the user if they want to initialize the data.
    /// If the user confirms, initializes the data using DalTest.Initialization.Do().
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event data.</param>
    private void Initialization_click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to initialization the data?", "Initialization", MessageBoxButton.YesNo))
        {
            DalTest.Initialization.Do();
        }
    }
}
