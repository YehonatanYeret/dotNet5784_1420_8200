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

    public double percentComplete
    {
        get { return (double)GetValue(percentCompleteProperty); }
        set
        {
            SetValue(percentCompleteProperty, value);
            OnPropertyChanged(nameof(percentComplete)); // Notify the UI about the property change
        }
    }
    public static readonly DependencyProperty percentCompleteProperty =
        DependencyProperty.Register("percentComplete", typeof(double), typeof(ManagerWindow), new PropertyMetadata(null));

    // Business logic layer instance
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Email { get; set; }
    public string ManagerName { get; set; }

    public ManagerWindow(string email)
    {
        Email = email;
        ManagerName = s_bl.User.Read(email).Name;
        int allTasks = s_bl.Task.ReadAll().Count();
        int completedTasks = s_bl.Task.ReadAll().Where(t => t.Status == BO.Status.Done).Count();
        percentComplete = ((double)completedTasks / allTasks) * 100;
        IsprojectStarted = (s_bl.Clock.GetProjectStatus() == BO.ProjectStatus.InProgress) ? Visibility.Hidden : Visibility.Visible;
        InitializeComponent();
    }

    private void EngineerButton_Click(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerListWindow().Show();
    }

    private void TaskButton_Click(object sender, RoutedEventArgs e)
    {
        new TaskListWindow().ShowDialog();
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
            //gets the date of the startProject from the dialog box
            DateTime? startProject = (DateTime?)dialogBox.Date;

            //start the project
            if (startProject is not null)
            {
                DateTime start = (DateTime)startProject;
                s_bl.Task.StartProject(start);
                if (s_bl.Clock.GetProjectStatus() == BO.ProjectStatus.InProgress)
                {
                    IsprojectStarted = Visibility.Hidden;
                }
            }
        }
    }

    // Invoke property changed event
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void BtnCreateManager_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            new CreateManagerWindow(0, Email, Email).ShowDialog();
            ManagerName = s_bl.User.Read(Email).Name;
        }
        catch
        {
            Close();
        }
    }
}
