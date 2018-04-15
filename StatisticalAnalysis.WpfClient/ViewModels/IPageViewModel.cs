namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public interface IPageViewModel
    {
        string Title { get; }

        void Search(object sender);
    }
}
