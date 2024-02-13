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

namespace PL.Messeges
{
    /// <summary>
    /// Interaction logic for IdInput.xaml
    /// </summary>
    public partial class IdInput : UserControl
    {
        public IdInput()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        public string Title { get; set; }

        public int MaxLength { get; set; }

    }
}
