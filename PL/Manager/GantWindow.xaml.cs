using PL.Task;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for GantWindow.xaml
    /// </summary>
    public partial class GantWindow : Window
    {

        /// <summary>
        /// Dependency Property for list of Tasks
        /// </summary>
        public IEnumerable<BO.Task> GantData
        {
            get { return (IEnumerable<BO.Task>)GetValue(GantDataProperty); }
            set { SetValue(GantDataProperty, value); }
        }

        /// <summary>
        /// Dependency property for TaskList
        /// </summary>
        public static readonly DependencyProperty GantDataProperty =
            DependencyProperty.Register("GantData", typeof(IEnumerable<BO.Task>), typeof(GantWindow), new PropertyMetadata(null));

        /// <summary>
        /// Dependency Property for list of Tasks
        /// </summary>
        public DateTime Now
        {
            get { return (DateTime)GetValue(NowProperty); }
            set { SetValue(NowProperty, value); }
        }

        /// <summary>
        /// Dependency property for TaskList
        /// </summary>
        public static readonly DependencyProperty NowProperty =
            DependencyProperty.Register("Now", typeof(DateTime), typeof(GantWindow), new PropertyMetadata(null));


        public GantWindow()
        {
            Now = BlApi.Factory.Get().Time;
            InitializeComponent();
            GantData = BlApi.Factory.Get().Task.GetTopologicalTasks();
            //GantData = BlApi.Factory.Get().Task.ReadAllTask().OrderBy(task=> task.ScheduledDate);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            BO.Task task = (BO.Task)((Grid)sender).DataContext;
            new TaskWindow(task.Id).ShowDialog();
        }
    }
}
