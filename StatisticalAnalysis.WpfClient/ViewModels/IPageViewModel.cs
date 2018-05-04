namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public interface IPageViewModel
    {
        string Title { get; }

        bool IsBusy { get; set; }

        void Search(object sender);
    }
}
