namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class PageViewModel : ValidationViewModelBase, IPageViewModel
    {
        public string Title { get; }

        public bool IsBusy
        {
            get => Get(() => IsBusy);
            set => Set(() => IsBusy, value);
        }

        public PageViewModel(string title)
        {
            Title = title;
        }

        public virtual void Search(object sender)
        {
            
        }
    }
}
