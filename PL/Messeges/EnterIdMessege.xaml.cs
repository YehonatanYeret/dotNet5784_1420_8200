namespace PL.Messeges;

using System.Windows;

/// <summary>
/// Interaction logic for EnterIdMessege.xaml
/// </summary>
public partial class EnterIdMessege : Window
{
    public string? Answer { get; set; }
    public string Question { get; set; }

    public EnterIdMessege(string text)
    {
        Question = text;
        InitializeComponent();
    }

    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
