using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для StatsAnalysis.xaml
    /// </summary>
    public partial class StatsAnalysis : UserControl, IView<INavigationViewModel>
    {
        public INavigationViewModel ViewModel
        {
            get => DataContext as INavigationViewModel;
            set => DataContext = value;
        }

        public StatsAnalysis(INavigationViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
