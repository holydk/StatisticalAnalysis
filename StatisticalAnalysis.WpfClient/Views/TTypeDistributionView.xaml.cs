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

            if (viewModel is ViewModelBase viewModelBase)
            {
                viewModelBase.PropertyChanged -= ViewModelBase_PropertyChanged;
                viewModelBase.PropertyChanged += ViewModelBase_PropertyChanged;
            }
        }

        private void ViewModelBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VariationData")
            {
                PART_InputDataContainer.Visibility = System.Windows.Visibility.Collapsed;
                PART_InputDataContainer.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
