﻿namespace PL.Manager;
using PL.Task;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for ManagerWindow.xaml
/// </summary>
public partial class ManagerWindow : Window, INotifyPropertyChanged
{
    public Visibility IsprojectStarted
    {
        get { return (Visibility)GetValue(IsprojectStartedProperty); }
        set
        {
            SetValue(IsprojectStartedProperty, value);
            OnPropertyChanged(nameof(IsprojectStarted)); // Notify the UI about the property change
        }
    }

    // Dependency property for CurrentEngineer
    public static readonly DependencyProperty IsprojectStartedProperty =
        DependencyProperty.Register("IsprojectStarted", typeof(Visibility), typeof(ManagerWindow), new PropertyMetadata(null));


    // Business logic layer instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public event PropertyChangedEventHandler? PropertyChanged;

    public ManagerWindow()
    {
        IsprojectStarted = (s_bl.Clock.GetProjectStatus() == BO.ProjectStatus.InProgress)? Visibility.Hidden: Visibility.Visible;
        InitializeComponent();
    }

    private void EngineerButton_Click(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerListWindow().Show();
    }

    private void TaskButton_Click(object sender, RoutedEventArgs e)
    {
        TaskListWindow taskListWindow = new TaskListWindow();
        taskListWindow.Owner = this;
        taskListWindow.setupWindow();
        taskListWindow.Show();
    }

    private void ResetDataBtn_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to reset all the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
        {
            s_bl.ResetDB();
            IsprojectStarted = Visibility.Visible;
        }

    }

    private void InitializeDataBtn_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Do you want to initialization the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
        {
            s_bl.InitializeDB();
            IsprojectStarted = Visibility.Visible;
        }
    }

    private void GanttClick_Button(object sender, RoutedEventArgs e)
    {
        new GantWindow().Show();
    }

    private void StartProject_Click(object sender, RoutedEventArgs e)
    {
        //ask for date
        var dialogBox = new Messeges.PickDate();

        //if the user chose a date set all dates
        if (dialogBox.ShowDialog() == true)
        {
            DateTime startProject = (DateTime)dialogBox.Date!;

            foreach (var item in s_bl.Task.ReadAllTask(task => task.ScheduledDate == null))
            {

                // calculate the closest date based on the startDate of the program and the depndencies 
                DateTime closest = s_bl.Task.CalculateClosestStartDate(item.Id, startProject);
                s_bl.Task.UpdateScheduledDate(item.Id, closest);
            }
            //start the project
            s_bl.Clock.SetStartProject(startProject);
            IsprojectStarted = Visibility.Hidden;
        }
    }

    // Invoke property changed event
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
