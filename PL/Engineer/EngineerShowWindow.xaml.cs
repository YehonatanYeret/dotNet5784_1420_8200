namespace PL.Engineer;

using System.Windows;

/// <summary>
/// Interaction logic for EngineerShowWindow.xaml
/// </summary>
public partial class EngineerShowWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public string? EngineerName
    {
        get { return (string?)GetValue(EngineerNameProperty); }
        set { SetValue(EngineerNameProperty, value); }
    }

    public static readonly DependencyProperty EngineerNameProperty =
        DependencyProperty.Register("EngineerName", typeof(string), typeof(EngineerShowWindow), new PropertyMetadata(null));

    public EngineerShowWindow(int id)
    {
        var engineer = s_bl.Engineer.Read(id);

        // Check if the engineer or engineer's name is null
        EngineerName = engineer?.Name;

        InitializeComponent();
    }
}
