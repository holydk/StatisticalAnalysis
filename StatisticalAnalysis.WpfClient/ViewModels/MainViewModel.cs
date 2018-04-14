using MaterialDesignThemes.Wpf;
using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.Views;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class MainViewModel : NavigationPageViewModel
    {
        public ILink[] Links { get; }

        public MainViewModel(INavigation navigation, string title)
            : base(navigation, title)
        {
            var homeVm = new HomeViewModel();
            Navigation.Add(() => new Home(homeVm));

            var statsAnalysisVm = new StatsAnalysisManagerViewModel(Navigation);
            Navigation.Add(() => new StatsAnalysis(statsAnalysisVm));

            _pagesViewModels = new IPageViewModel[]
            {
                homeVm, statsAnalysisVm
            };

            _navigationItems = new INavigationItem[]
            {
                new NavigationItem(homeVm.Title, typeof(Home), PackIconKind.Home),
                new NavigationItem(statsAnalysisVm.Title, typeof(StatsAnalysis), PackIconKind.ChartBar)
            };

            Links = new ILink[]
            {
                new Link("GitHub", "https://github.com/holydk/StatisticalAnalysis", PackIconKind.GithubCircle, "Исходный код")
            };
        }
    }
}
