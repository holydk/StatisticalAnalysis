namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class PageViewModel : ValidationViewModelBase, IPageViewModel
    {
        public string Title { get; }

        public PageViewModel(string title)
        {
            Title = title;
        }

        public virtual void Search(object sender)
        {
            
        }
    }
}
