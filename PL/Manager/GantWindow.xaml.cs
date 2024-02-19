using PL.Task;
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


        public GantWindow()
        {
            InitializeComponent();
            GantData = BlApi.Factory.Get().Task.ReadAllTask().OrderBy(task=> task.ScheduledDate);
        }

    }
}
