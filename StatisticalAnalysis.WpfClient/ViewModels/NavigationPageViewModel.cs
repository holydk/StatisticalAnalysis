using StatisticalAnalysis.WpfClient.Models;
using System;
using System.Collections.Generic;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class NavigationPageViewModel : InformationPageViewModel, INavigationViewModel
    {
        protected IPageViewModel[] _pagesViewModels;

        public INavigation Navigation { get; }

        protected IEnumerable<INavigationItem> _navigationItems;
        public IEnumerable<INavigationItem> NavigationItems => _navigationItems;

        public NavigationPageViewModel(INavigation navigation, string title)
            : base(title)
        {
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        }
    }
}
