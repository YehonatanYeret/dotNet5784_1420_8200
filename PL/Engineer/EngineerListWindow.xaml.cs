namespace PL.Engineer;

using BlApi;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    // Business logic layer instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Dependency Property for list of Engineers
    /// </summary>
    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    /// <summary>
    /// Dependency property for EngineerList
    /// </summary>
    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Selected engineer's experience level
    /// </summary>
    public BO.EngineerExperience experience { get; set; } = BO.EngineerExperience.None;

    /// <summary>
    /// if the window is for choosing an engineer
    /// </summary>
    public int ChooseEngineer { get; set; }

    /// <summary>
    /// Constructor for EngineerListWindow
    /// </summary>
    public EngineerListWindow(int taskId = 0)
    {
        InitializeComponent();
        EngineerList = s_bl.Engineer.ReadAll()!;
        ChooseEngineer = taskId;
    }

    /// <summary>
    /// Event handler for selection changed in the ComboBox
    /// </summary>
    private void Expirience_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateListView();
    }

    /// <summary>
    /// Event handler for the "Add" button click
    /// </summary>
    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
            new Engineer.EngineerWindow().ShowDialog();
            UpdateListView();   
    }

    /// <summary>
    /// Event handler for double click on the ListView item
    /// </summary>
    private void UpdateListView_DoubleClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ChooseEngineer == 0)
            {
                BO.Engineer? EngineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
                if (EngineerInList != null)
                {
                    new Engineer.EngineerWindow(EngineerInList!.Id).ShowDialog();
                    UpdateListView();
                }
            }
            else
            {
                BO.Task task = s_bl.Task.Read(ChooseEngineer);
                BO.Engineer? EngineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
                task.Engineer = Factory.Get().Engineer.GetEngineerInTask(EngineerInList!.Id);
                s_bl.Task.Update(task);
                Close();
            }
        }
        catch(Exception ex)
        {
            MessageBox.Show("error aqured", ex.Message);
        }
    }


    /// <summary>
    /// Updates the ListView based on the selected experience level
    /// </summary>
    void UpdateListView()
    {
        EngineerList = (experience == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()!.OrderBy(item => item.Name)! :
            s_bl?.Engineer.ReadAll(item => item.Level == experience)!.OrderBy(item => item.Name)!;
    }
}