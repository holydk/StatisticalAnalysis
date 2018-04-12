using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.Views;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class StatsAnalysisManagerViewModel : NavigationPageViewModel
    {
        public StatsAnalysisManagerViewModel(INavigation navigation)
            : base(navigation, "Статистический анализ")
        {
            var hypTestingVm = new HypothesisTestingManagerViewModel(navigation);
            Navigation.Add(() => new HypothesisTestingView(hypTestingVm));

            _pagesViewModels = new IPageViewModel[]
            {
                hypTestingVm
            };

            _navigationItems = new INavigationItem[]
            {
                new NavigationItem(hypTestingVm.Title, typeof(HypothesisTestingView), MaterialDesignThemes.Wpf.PackIconKind.FormatListChecks)
            };
        }
    }
}
