namespace PL.Engineer;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Dependency Property for list of Engineer
    /// </summary>
    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    public BO.EngineerExperience experience { get; set; } = BO.EngineerExperience.None;

    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl.Engineer.ReadAll()!;
    }

    private void Expirience_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        UpdateListView();
    }

    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerWindow().ShowDialog();
        UpdateListView();

    }
    private void UpdateListView_DoubleClick(object sender, RoutedEventArgs e)
    {
        BO.Engineer? EngineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
        new Engineer.EngineerWindow(EngineerInList!.Id).ShowDialog();
        UpdateListView();
    }
    void UpdateListView()
    {
        EngineerList = (experience == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()!.OrderBy(item=> item.Name)! :
            s_bl?.Engineer.ReadAll(item => item.Level == experience)!.OrderBy(item => item.Name)!;
    }
}

