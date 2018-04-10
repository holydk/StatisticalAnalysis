using StatisticalAnalysis.WpfClient.Models;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class MainViewModel : INavigationViewModel, IPageViewModel
    {
        public string Title => "MathStats";

        public INavigation Navigation { get; }

        public MainViewModel()
        {
            Navigation = new Navigation();
        }
    }
}
