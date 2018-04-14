using StatisticalAnalysis.WpfClient.ViewModels;
using System.Linq;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для HypothesisTestingView.xaml
    /// </summary>
    public partial class HypothesisTestingView : UserControl, IView<HypothesisTestingManagerViewModel>
    {
        public HypothesisTestingManagerViewModel ViewModel
        {
            get => DataContext as HypothesisTestingManagerViewModel;
            set => DataContext = value;
        }
    
        public HypothesisTestingView(HypothesisTestingManagerViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }

        private async void Button_Click1(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.Navigation.GoToAsync(ViewModel.NavigationItems.First().ViewType);
        }
    }
}
