namespace PL.Manager;

using System.Windows;
using System.Windows.Controls;


/// <summary>
/// Interaction logic for CreateManagerWindow.xaml
/// </summary>
public partial class CreateManagerWindow : Window
{
    // Get the business logic instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.User CurrentManager
    {
        get { return (BO.User)GetValue(userProperty); }
        set { SetValue(userProperty, value); }
    }

    public static readonly DependencyProperty userProperty =
        DependencyProperty.Register("CurrentManager", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));
    public CreateManagerWindow()
    {
        CurrentManager = new BO.User();
        InitializeComponent();
    }

    private void BtnCreateManager_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CurrentManager.Type = BO.UserType.manager;
            s_bl.User.Create(CurrentManager);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void PasswordBox_PasswordChange(object sender, RoutedEventArgs e)
    {
        CurrentManager.Password = ((PasswordBox)sender).Password;
    }
}
