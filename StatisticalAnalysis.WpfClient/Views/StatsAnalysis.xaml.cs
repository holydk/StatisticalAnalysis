using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для StatsAnalysis.xaml
    /// </summary>
    public partial class StatsAnalysis : UserControl, IView<StatsAnalysisMenegerViewModel>
    {
        public StatsAnalysisMenegerViewModel ViewModel
        {
            get => DataContext as StatsAnalysisMenegerViewModel;
            set => DataContext = value;
        }

        public StatsAnalysis(StatsAnalysisMenegerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.Navigation.GoToAsync(ViewModel.NavigationItems[0].ViewType);
        }
    }
}
