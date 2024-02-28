namespace PL.Manager;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for ManagerListWindowxaml.xaml
/// </summary>
public partial class ManagerListWindow : Window
{
    // Business logic layer instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Dependency Property for list of Engineers
    /// </summary>
    public IEnumerable<BO.User> ManagerList
    {
        get { return (IEnumerable<BO.User>)GetValue(ManagerListProperty); }
        set { SetValue(ManagerListProperty, value); }
    }

    /// <summary>
    /// Dependency property for EngineerList
    /// </summary>
    public static readonly DependencyProperty ManagerListProperty =
        DependencyProperty.Register("ManagerList", typeof(IEnumerable<BO.User>), typeof(ManagerListWindow), new PropertyMetadata(null));


    public string Email { get; set; }

    public ManagerListWindow(string email)
    {
        Email = email;
        ManagerList = s_bl.User.ReadAll(user => user.Type == BO.UserType.manager)!;
        InitializeComponent();
    }


    /// <summary>
    /// Event handler for the "Add" button click
    /// </summary>
    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
        new CreateManagerWindow().ShowDialog();
        UpdateListView();
    }


    /// <summary>
    /// Event handler for double click on the ListView item
    /// </summary>
    private void UpdateListView_DoubleClick(object sender, RoutedEventArgs e)
    {
        try
        {
            BO.User? UserInList = (sender as ListView)?.SelectedItem as BO.User;

            if (UserInList != null)
            {
                new CreateManagerWindow(0, Email, UserInList!.Email).ShowDialog();
                UpdateListView();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("error aqured", ex.Message);
        }
    }

    /// <summary>
    /// Updates the ListView 
    /// </summary>
    void UpdateListView()
    {
        ManagerList = s_bl?.User.ReadAll(user => user.Type == BO.UserType.manager)!.OrderBy(item => item.Name)!;
    }
}
