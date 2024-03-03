namespace PL.Messeges;

using System.Windows;

/// <summary>
/// Interaction logic for PickDate.xaml
/// </summary>
public partial class PickDate : Window
{
    // Property representing the selected date
    public DateTime? Date { get; set; }

    // Constructor for PickDate window
    public PickDate()
    {
        // Initializing components (from XAML)
        InitializeComponent();
    }

    // Event handler for the "OK" button click
    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
        // Set DialogResult to true to indicate OK button click
        DialogResult = true;
    }
}
