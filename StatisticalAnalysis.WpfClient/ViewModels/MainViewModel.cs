using MaterialDesignThemes.Wpf;
using StatisticalAnalysis.WpfClient.Commands;
using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.Views;
using System.Windows.Input;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class MainViewModel : INavigationViewModel
    {
        private IPageViewModel[] PagesViewModels;

        public INavigation Navigation { get; }

        public INavigationItem[] NavigationItems { get; }

        public ILink[] Links { get; }

        public ICommand OpenLinkCommand { get; }

        public MainViewModel()
        {
            Navigation = new Navigation();

            var homeVm = new HomeViewModel();
            Navigation.Add(() => new Home(homeVm));

            var statsAnalysisVm = new StatsAnalysisMenegerViewModel(Navigation);
            Navigation.Add(() => new StatsAnalysis(statsAnalysisVm));

            PagesViewModels = new IPageViewModel[]
            {
                homeVm, statsAnalysisVm
            };

            NavigationItems = new INavigationItem[]
            {
                new NavigationItem(homeVm.Title, typeof(Home), PackIconKind.Home),
                new NavigationItem(statsAnalysisVm.Title, typeof(StatsAnalysis), PackIconKind.ChartBar)
            };

            Links = new ILink[]
            {
                new Link("GitHub", "https://github.com/holydk/StatisticalAnalysis", PackIconKind.GithubCircle, "Исходный код")
            };

            OpenLinkCommand = new RelayCommand((sender) =>
            {
                if (sender is ILink link)
                {
                    System.Diagnostics.Process.Start(link.Adress);
                }
            });
        }
    }
}
