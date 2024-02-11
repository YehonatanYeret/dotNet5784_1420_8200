namespace PL;

using System.Windows;


/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TaskList_click(object sender, RoutedEventArgs e)
    {
        new Task.TaskListWindow().Show();
    }

    private void EngineerList_click(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerListWindow().Show();
    }

    private void Initialization_click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to initialization the data?", "Initialization", MessageBoxButton.YesNo))
        {
            DalTest.Initialization.Do();
        }
    }
}