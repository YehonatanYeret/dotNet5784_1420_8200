namespace PL.Manager;

using BlApi;
using PL.Task;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// Interaction logic for GantWindow.xaml
/// </summary>
public partial class GantWindow : Window, INotifyPropertyChanged
{
    /// <summary>
    /// Dependency Property for the list of Tasks in Gantt Chart
    /// </summary>
    public IEnumerable<BO.Task> GantData
    {
        get { return (IEnumerable<BO.Task>)GetValue(GantDataProperty); }
        set
        {
            SetValue(GantDataProperty, value);
            OnPropertyChanged(nameof(GantData));
        }
    }

    /// <summary>
    /// Dependency property for the Gantt Chart data
    /// </summary>
    public static readonly DependencyProperty GantDataProperty =
        DependencyProperty.Register("GantData", typeof(IEnumerable<BO.Task>), typeof(GantWindow), new PropertyMetadata(null));

    /// <summary>
    /// Dependency Property for the current date and time in Gantt Chart
    /// </summary>
    public DateTime Now
    {
        get { return (DateTime)GetValue(NowProperty); }
        set
        {
            SetValue(NowProperty, value);
            OnPropertyChanged(nameof(Now));
        }
    }

    /// <summary>
    /// Dependency property for the current date and time
    /// </summary>
    public static readonly DependencyProperty NowProperty =
        DependencyProperty.Register("Now", typeof(DateTime), typeof(GantWindow), new PropertyMetadata(null));

    // Event for property changed
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Constructor for GantWindow
    /// </summary>
    public GantWindow()
    {
        Activated += GantWindow_Activated; // Event handler for window activation
        InitializeComponent();
    }

    /// <summary>
    /// Event handler for window activation
    /// </summary>
    private void GantWindow_Activated(object? sender, EventArgs e)
    {
        // Load Gantt Chart data and set the current date and time
        GantData = BlApi.Factory.Get().Task.GetTopologicalTasks();
        Now = Factory.Get().Time;
    }

    /// <summary>
    /// Event handler for mouse leave on the Gantt Chart task grid
    /// </summary>
    private void Grid_MouseLeave(object sender, MouseEventArgs e)
    {
        // Open the TaskWindow for the selected task on mouse leave
        BO.Task task = (BO.Task)((Grid)sender).DataContext;
        new TaskWindow(task.Id).ShowDialog();
    }

    /// <summary>
    /// Invoke property changed event
    /// </summary>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
