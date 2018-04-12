using StatisticalAnalysis.WpfClient.Models;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public interface INavigationViewModel
    {
        INavigation Navigation { get; }

        INavigationItem[] NavigationItems { get; }
    }
}
