using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.Views;
using System;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class StatsAnalysisMenegerViewModel : IPageViewModel, INavigationViewModel
    {
        private IPageViewModel[] PagesViewModels;

        public string Title => "Статистический анализ";

        public INavigation Navigation { get; }

        public INavigationItem[] NavigationItems { get; }

        public StatsAnalysisMenegerViewModel(INavigation navigation)
        {
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            var hypTestingVm = new HypothesisTestingViewModel();
            Navigation.Add(() => new HypothesisTestingView(hypTestingVm));

            PagesViewModels = new IPageViewModel[]
            {
                hypTestingVm
            };

            NavigationItems = new INavigationItem[]
            {
                new NavigationItem(hypTestingVm.Title, typeof(HypothesisTestingView), MaterialDesignThemes.Wpf.PackIconKind.FormatListChecks)
            };
        }
    }
}
