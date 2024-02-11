namespace PL.Engineer;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;


/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Dependency Property for list of Engineer
    /// </summary>
    public ObservableCollection<BO.Engineer> EngineerList
    {
        get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    public BO.EngineerExperience experience { get; set; } = BO.EngineerExperience.None;

    public EngineerListWindow()
    {
        EngineerList = new ObservableCollection<BO.Engineer>(s_bl.Engineer.ReadAll()!);
        InitializeComponent();
    }

    private void Expirience_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        EngineerList = (experience == BO.EngineerExperience.None) ?
           new ObservableCollection<BO.Engineer>( s_bl?.Engineer.ReadAll()!) :
           new ObservableCollection<BO.Engineer>(s_bl?.Engineer.ReadAll(item => item.Level == experience)!);

    }

    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerWindow().ShowDialog(); 
    }
    private void UpdateListView_DoubleClick(object sender, RoutedEventArgs e)
    {
        BO.Engineer? EngineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
        new Engineer.EngineerWindow(EngineerInList!.Id).ShowDialog();
    }
}

