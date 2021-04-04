using MavcaDetection.ViewModels;
using System.Windows.Controls;

namespace MavcaDetection.Controls
{
    /// <summary>
    /// Interaction logic for ControlBarUC.xaml
    /// </summary>
    public partial class ControlBarUC : UserControl
    {
        public ControlBarViewModel ViewModel { get; set; }
        public ControlBarUC()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ControlBarViewModel();
        }
    }
}
