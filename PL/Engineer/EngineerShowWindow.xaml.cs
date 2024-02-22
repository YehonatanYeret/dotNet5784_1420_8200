namespace PL.Engineer;

using PL.Task;
using System.Windows;

/// <summary>
/// Interaction logic for EngineerShowWindow.xaml
/// </summary>
public partial class EngineerShowWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerShowWindow), new PropertyMetadata(null));

    public BO.TaskInList? Task { get; set; }

    public EngineerShowWindow(int id)
    {
        CurrentEngineer = s_bl.Engineer.Read(id);

        if (CurrentEngineer.Task != null)
            Task = s_bl.Task.ConvertToTaskInList(CurrentEngineer.Task.Id);
        else
            Task = new BO.TaskInList() { Id = 0};
        InitializeComponent();
    }

    private void ChooseTaskBtn_Click(object sender, RoutedEventArgs e)
    {
        TaskListWindow taskListWindow = new TaskListWindow();
        taskListWindow.Owner = this;
        if(taskListWindow.setupWindow(CurrentEngineer!.Id))
            taskListWindow.Show();
    }

    private void FinishTaskBtn_Click(object sender, RoutedEventArgs e)
    {
        s_bl.Task.ChangeStatusOfTask(Task!.Id);
    }
}
