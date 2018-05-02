using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для TTypeDistributionView.xaml
    /// </summary>
    public partial class TTypeDistributionView : UserControl, IView
    {
        public IPageViewModel ViewModel
        {
            get => DataContext as IPageViewModel;
            set => DataContext = value;
        }
    
        public TTypeDistributionView(IPageViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            if (viewModel is TTypeDistributionViewModel distrVM)
            {
                distrVM.VariationData = null;
                distrVM.SelectedSignificanceLevel = null;
                distrVM.SelectedDistributionType = null;
                distrVM.SelectedDistributionSeriesInputType = null;
            }
        }
    }
}
