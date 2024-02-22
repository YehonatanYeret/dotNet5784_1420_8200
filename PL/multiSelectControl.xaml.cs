using BlApi;
using BO;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL;




/// <summary>
/// Interaction logic for multiSelectControl.xaml
/// </summary>
public partial class multiSelectControl : UserControl
{
    public IEnumerable<TaskInList> Data
    {
        get { return (IEnumerable<TaskInList>)GetValue(DataProperty); }
        set { SetValue(DataProperty, value); }
    }

    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register("Data", typeof(IEnumerable<TaskInList>), typeof(multiSelectControl), new PropertyMetadata(null));


    public multiSelectControl()
    {
        Data = from t in Factory.Get().Task.ReadAll()
               select new TaskInList()
               {
                   Id = t.Id,
                   Description = t.Description,
                   Alias = t.Alias,
                   Status = t.Status,
                   IsChecked = true
               };
        InitializeComponent();
    }
}

/// <summary>
/// Represents a task in the list of the dependencies of another task.
/// </summary>
public class TaskInList
{
    /// <summary>
    /// Gets or initializes the unique identifier for the task.
    /// </summary>
    /// <remarks>
    /// This property serves as the primary key and references BO.Task.Id.
    /// </remarks>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    /// <remarks>
    /// The description cannot be null.
    /// </remarks>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the alias of the task.
    /// </summary>
    /// <remarks>
    /// The alias cannot be null.
    /// </remarks>
    public string Alias { get; set; }

    /// <summary>
    /// Gets or sets the status of the task.
    /// </summary>
    /// <remarks>
    /// The status is calculated.
    /// </remarks>
    public Status? Status { get; set; }

    public bool IsChecked { get; set; }

    public override string ToString() => this.ToStringProperty();
}