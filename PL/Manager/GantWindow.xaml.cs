using BlApi;
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
    public partial class GantWindow : Window, INotifyPropertyChanged
    {

        /// <summary>
        /// Dependency Property for list of Tasks
        /// </summary>
        public IEnumerable<BO.Task> GantData
        {
            get { return (IEnumerable<BO.Task>)GetValue(GantDataProperty); }
            set { SetValue(GantDataProperty, value);
                OnPropertyChanged(nameof(GantData));
            }
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
            set { SetValue(NowProperty, value);
                OnPropertyChanged(nameof(Now));
            }
        }

        /// <summary>
        /// Dependency property for TaskList
        /// </summary>
        public static readonly DependencyProperty NowProperty =
            DependencyProperty.Register("Now", typeof(DateTime), typeof(GantWindow), new PropertyMetadata(null));

        public event PropertyChangedEventHandler? PropertyChanged;

        public GantWindow()
        {
            Activated += GantWindow_Activated;
            InitializeComponent();
        }


        private void GantWindow_Activated(object? sender, EventArgs e)
        {
            GantData = BlApi.Factory.Get().Task.GetTopologicalTasks();
            Now = Factory.Get().Time;
        }
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            BO.Task task = (BO.Task)((Grid)sender).DataContext;
            new TaskWindow(task.Id).ShowDialog();
        }

        // Invoke property changed event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
