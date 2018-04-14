using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для CorRegAnalysisView.xaml
    /// </summary>
    public partial class CorRegAnalysisView : UserControl, IView
    {
        public IPageViewModel ViewModel
        {
            get => DataContext as IPageViewModel;
            set => DataContext = value;
        }

        public CorRegAnalysisView(IPageViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();
        }
    }
}
