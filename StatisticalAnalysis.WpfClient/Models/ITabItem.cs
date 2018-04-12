using MaterialDesignThemes.Wpf;

namespace StatisticalAnalysis.WpfClient.Models
{
    public interface ITabItem
    {
        string Title { get; }

        PackIconKind IconKind { get; }
    }
}
