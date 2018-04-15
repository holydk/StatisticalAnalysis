using StatisticalAnalysis.WpfClient.Commands;
using StatisticalAnalysis.WpfClient.Models;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class NavigationPageViewModel : InformationPageViewModel, INavigationViewModel
    {
        protected IPageViewModel[] _pagesViewModels;

        public INavigation Navigation { get; }

        public UserControl Content => Navigation.Content;

        protected IEnumerable<INavigationItem> _navigationItems;
        public IEnumerable<INavigationItem> NavigationItems => _navigationItems;

        public ICommand GoToCommand { get; }

        public ICommand GoBackToCommand { get; }

        public NavigationPageViewModel(INavigation navigation, string title)
            : base(title)
        {
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            GoToCommand = new RelayCommand(async (sender) =>
            {
                if (sender is Type viewType)
                {
                    await Navigation.GoToAsync(viewType);
                }
            });

            GoBackToCommand = new RelayCommand(async (sender) =>
            {
                if (sender is Type viewType)
                {
                    await Navigation.GoBackAsync(viewType);
                }
            });
        }
    }
}
