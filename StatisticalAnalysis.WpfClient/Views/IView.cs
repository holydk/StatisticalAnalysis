using StatisticalAnalysis.WpfClient.ViewModels;

namespace StatisticalAnalysis.WpfClient.Views
{
    public interface IView<T> where T : class, IPageViewModel
    {
        T ViewModel { get; set; }
    }

    public interface IView
    {
        IPageViewModel ViewModel { get; set; }
    }
}
