using StatisticalAnalysis.WpfClient.Models;
using System;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class NavigationPageViewModel : InformationPageViewModel, INavigationViewModel
    {
        protected IPageViewModel[] _pagesViewModels;

        public INavigation Navigation { get; }

        protected INavigationItem[] _navigationItems;
        public INavigationItem[] NavigationItems => _navigationItems;

        public NavigationPageViewModel(INavigation navigation, string title)
            : base(title)
        {
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        }
    }
}
