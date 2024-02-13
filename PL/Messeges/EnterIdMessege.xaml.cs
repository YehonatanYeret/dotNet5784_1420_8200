using PL.Engineer;
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

namespace PL.Messeges
{
    /// <summary>
    /// Interaction logic for EnterIdMessege.xaml
    /// </summary>
    public partial class EnterIdMessege : Window
    {
        public string? AnswerId { get; set; }

        public EnterIdMessege()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
