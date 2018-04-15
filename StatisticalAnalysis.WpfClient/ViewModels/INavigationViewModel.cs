using StatisticalAnalysis.WpfClient.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public interface INavigationViewModel : IInformationPageViewModel
    {
        INavigation Navigation { get; }

        IEnumerable<INavigationItem> NavigationItems { get; }

        ICommand GoToCommand { get; }

        ICommand GoBackToCommand { get; }
    }
}
