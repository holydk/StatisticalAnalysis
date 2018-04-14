using StatisticalAnalysis.WpfClient.Models;
using System.Collections.Generic;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public interface INavigationViewModel
    {
        INavigation Navigation { get; }

        IEnumerable<INavigationItem> NavigationItems { get; }
    }
}
