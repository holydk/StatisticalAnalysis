using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для HypothesisTestingView.xaml
    /// </summary>
    public partial class HypothesisTestingView : UserControl, IView<HypothesisTestingViewModel>
    {
        public HypothesisTestingViewModel ViewModel
        {
            get => DataContext as HypothesisTestingViewModel;
            set => DataContext = value;
        }
    
        public HypothesisTestingView(HypothesisTestingViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
