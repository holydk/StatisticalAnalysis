using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для TTypeDistributionView.xaml
    /// </summary>
    public partial class TTypeDistributionView : UserControl, IView<TTypeDistributionViewModel>
    {
        public TTypeDistributionViewModel ViewModel
        {
            get => DataContext as TTypeDistributionViewModel;
            set => DataContext = value;
        }
    
        public TTypeDistributionView(TTypeDistributionViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
