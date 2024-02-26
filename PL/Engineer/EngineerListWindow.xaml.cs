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


    public BO.EngineerInTask? choosenEngineer { get; set; }

    /// <summary>
    /// if the window is for choosing an engineer
    /// </summary>
    public int TaskID {
    get { return (int)GetValue(TaskIDProperty); }
        set { SetValue(TaskIDProperty, value); }
    }

    public static readonly DependencyProperty TaskIDProperty =
        DependencyProperty.Register("TaskID", typeof(int), typeof(EngineerListWindow), new PropertyMetadata(0));

    /// <summary>
    /// Constructor for EngineerListWindow
    /// </summary>
    public EngineerListWindow(int taskId = 0)
    {
        EngineerList = s_bl.Engineer.ReadAll()!;
        TaskID = taskId;
        InitializeComponent();
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
            BO.Engineer? EngineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
            choosenEngineer = new BO.EngineerInTask() { Id = EngineerInList!.Id, Name = EngineerInList!.Name};
            if (TaskID == 0)
            {
                if (EngineerInList != null)
                {
                    new Engineer.EngineerWindow(EngineerInList!.Id).ShowDialog();
                    UpdateListView();
                }
            }
            else
            {

                //BO.Task task = s_bl.Task.Read(TaskID);
                //task.Engineer = s_bl.Engineer.GetEngineerInTask(EngineerInList!.Id);
                //s_bl.Task.Update(task);
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