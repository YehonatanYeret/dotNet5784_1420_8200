namespace PL.Messeges;

using System.Windows;

/// <summary>
/// Interaction logic for EnterIdMessege.xaml
/// </summary>
public partial class EnterIdMessege : Window
{
    public string? AnswerId { get; set; }

    public EnterIdMessege()
    {
        InitializeComponent();
    }

    private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
