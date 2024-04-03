namespace PL.Messeges;

using BlApi;
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
        // Start the project
        if (Date is not null)
        {
            Factory.Get().Task.StartProject((DateTime)Date);
            Close();
        }
    }
}
