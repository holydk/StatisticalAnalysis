using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для ForecastingTimeSeriesView.xaml
    /// </summary>
    public partial class ForecastingTimeSeriesView : UserControl, IView
    {
        public IPageViewModel ViewModel
        {
            get => DataContext as IPageViewModel;
            set => DataContext = value;
        }

        public ForecastingTimeSeriesView(IPageViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
