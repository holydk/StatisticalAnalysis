using MaterialDesignThemes.Wpf;

namespace StatisticalAnalysis.WpfClient.Models
{
    public interface IImageItem : ITabItem
    {
        PackIconKind IconKind { get; }
    }
}
