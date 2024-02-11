namespace PL.Engineer;

using System.Windows;


/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Dependency Property for list of Engineer
    /// </summary>
    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    public BO.EngineerExperience experience { get; set; } = BO.EngineerExperience.None;

    public EngineerListWindow()
    {
        InitializeComponent();

        EngineerList = s_bl?.Engineer.ReadAll()!;
    }

    private void Expirience_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        EngineerList = (experience == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == experience)!;
    }
}

