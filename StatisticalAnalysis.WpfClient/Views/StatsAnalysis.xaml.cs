using StatisticalAnalysis.WpfClient.ViewModels;
using System.Linq;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для StatsAnalysis.xaml
    /// </summary>
    public partial class StatsAnalysis : UserControl, IView<StatsAnalysisManagerViewModel>
    {
        public StatsAnalysisManagerViewModel ViewModel
        {
            get => DataContext as StatsAnalysisManagerViewModel;
            set => DataContext = value;
        }

        public StatsAnalysis(StatsAnalysisManagerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.Navigation.GoToAsync(ViewModel.NavigationItems.First().ViewType);
        }
    }
}
