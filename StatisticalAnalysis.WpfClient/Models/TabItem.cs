using MaterialDesignThemes.Wpf;

namespace StatisticalAnalysis.WpfClient.Models
{
    public class TabItem : ITabItem
    {
        public string Title { get; }

        public PackIconKind IconKind { get; }

        public TabItem(string title, PackIconKind iconKind)
        {
            Title = title;
            IconKind = iconKind;
        }
    }
}
