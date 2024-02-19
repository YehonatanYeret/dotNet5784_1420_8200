namespace PL.Engineer;

using BlApi;
using System.Windows;


/// <summary>
/// Interaction logic for ShowEngineer.xaml
/// </summary>
public partial class ShowEngineer : Window
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Dependency Property for Engineer
    /// </summary>
    public BO.Engineer Engineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


    public ShowEngineer(int id)
    {
        Engineer = Factory.Get().Engineer.Read(id);
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        //stupid code that can be better
        new Engineer.EngineerWindow(Engineer.Id).ShowDialog();
        Engineer = s_bl.Engineer.Read(Engineer.Id);
        new Engineer.ShowEngineer(Engineer.Id).Show();
        Close();
    }
}
