namespace PL.Engineer;

using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

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
    public int TaskID
    {
        get { return (int)GetValue(TaskIDProperty); }
        set { SetValue(TaskIDProperty, value); }
    }

    public static readonly DependencyProperty TaskIDProperty =
        DependencyProperty.Register("TaskID", typeof(int), typeof(EngineerListWindow), new PropertyMetadata(0));

    /// <summary>
    /// The level of the task
    /// </summary>
    public BO.EngineerExperience TaskLevel { get; set; }

    /// <summary>
    /// Constructor for EngineerListWindow
    /// </summary>
    /// <param name="taskId">The ID of the task</param>
    /// <param name="taskLevel">The experience level of the task</param>
    public EngineerListWindow(int taskId = 0, BO.EngineerExperience taskLevel = BO.EngineerExperience.None)
    {
        EngineerList = (taskId == 0) ? s_bl.Engineer.ReadAll()! :
            s_bl.Engineer.ReadAll(eng => eng.Level >= taskLevel)!;
        TaskID = taskId;
        TaskLevel = taskLevel;
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for selection changed in the ComboBox
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">The event arguments</param>
    private void Expirience_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateListView();
    }

    /// <summary>
    /// Event handler for the "Add" button click
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">The event arguments</param>
    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
        // Open the EngineerWindow for adding a new engineer
        new Engineer.EngineerWindow().ShowDialog();
        UpdateListView();
    }

    /// <summary>
    /// Event handler for double click on the ListView item
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">The event arguments</param>
    private void UpdateListView_DoubleClick(object sender, RoutedEventArgs e)
    {
        try
        {
            // Retrieve the selected engineer from the ListView
            BO.Engineer? EngineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;

            // Check if an engineer is selected
            if (EngineerInList is not null)
            {
                // Create a new EngineerInTask instance based on the selected engineer
                choosenEngineer = new BO.EngineerInTask() { Id = EngineerInList!.Id, Name = EngineerInList!.Name };

                // Check if the window is not for choosing an engineer
                if (TaskID == 0)
                {
                    // Open the EngineerWindow for the selected engineer
                    if (EngineerInList != null)
                    {
                        new Engineer.EngineerWindow(EngineerInList!.Id).ShowDialog();
                        UpdateListView();
                    }
                }
                else
                {
                    // Close the window when an engineer is chosen for a task
                    Close();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error occurred", ex.Message);
        }
    }

    /// <summary>
    /// Updates the ListView based on the selected experience level
    /// </summary>
    private void UpdateListView()
    {
        if (TaskID == 0)
        {
            EngineerList = (experience == BO.EngineerExperience.None) ?
                s_bl?.Engineer.ReadAll()!.OrderBy(item => item.Name)! :
                s_bl?.Engineer.ReadAll(item => item.Level == experience)!.OrderBy(item => item.Name)!;
        }
        else
        {
            EngineerList = (experience == BO.EngineerExperience.None) ?
                s_bl?.Engineer.ReadAll(eng => eng.Level >= TaskLevel)!.OrderBy(item => item.Name)! :
                s_bl?.Engineer.ReadAll(eng => eng.Level >= TaskLevel && eng.Level == experience)!.OrderBy(item => item.Name)!;
        }
    }
}
