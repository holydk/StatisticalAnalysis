using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для HypothesisTestingView.xaml
    /// </summary>
    public partial class HypothesisTestingView : UserControl, IView<INavigationViewModel>
    {
        public INavigationViewModel ViewModel
        {
            get => DataContext as INavigationViewModel;
            set => DataContext = value;
        }
    
        public HypothesisTestingView(INavigationViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
