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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for ShowEngineer.xaml
    /// </summary>
    public partial class ShowEngineer : Window
    {
        // Get the business logic instance
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Dependency Property for Engineer
        /// </summary>
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        public ShowEngineer(BO.Engineer engineer)
        {
            Engineer = engineer;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //stupid code that can be better
            new Engineer.EngineerWindow(Engineer.Id).ShowDialog();
            Engineer = s_bl.Engineer.Read(Engineer.Id);
            new Engineer.ShowEngineer(Engineer).Show();
            Close();
        }
    }
}
