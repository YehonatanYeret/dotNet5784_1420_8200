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

namespace PL.Messeges;

/// <summary>
/// Interaction logic for PickDate.xaml
/// </summary>
public partial class PickDate : Window
{
    public DateTime? Date {  get; set; }
    public PickDate()
    {
        InitializeComponent();
    }
    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
