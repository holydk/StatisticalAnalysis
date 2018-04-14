using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для TimeSeriesAnalysisView.xaml
    /// </summary>
    public partial class TimeSeriesAnalysisView : UserControl, IView
    {
        public IPageViewModel ViewModel
        {
            get => DataContext as IPageViewModel;
            set => DataContext = value;
        }

        public TimeSeriesAnalysisView(IPageViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
