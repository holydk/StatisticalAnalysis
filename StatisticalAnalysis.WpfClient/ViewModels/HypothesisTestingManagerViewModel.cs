using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.Views;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class HypothesisTestingManagerViewModel : NavigationPageViewModel, IPageViewModel
    {
        public HypothesisTestingManagerViewModel(INavigation navigation)
            : base(navigation, "Проверка гипотез")
        {
            var tTypeDistrVm = new TTypeDistributionViewModel();
            navigation.Add(() => new TTypeDistributionView(tTypeDistrVm));

            _pagesViewModels = new IPageViewModel[]
            {
                tTypeDistrVm
            };

            _navigationItems = new INavigationItem[]
            {
                new NavigationItem(tTypeDistrVm.Title, typeof(TTypeDistributionView), MaterialDesignThemes.Wpf.PackIconKind.FormatListChecks)
            };
        }
    }
}
