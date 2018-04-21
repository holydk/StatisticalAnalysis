namespace StatisticalAnalysis.WpfClient.Models
{
    public class TabItem : ITabItem
    {
        public string Title { get; }

        public TabItem(string title)
        {
            Title = title;
        }
    }
}
