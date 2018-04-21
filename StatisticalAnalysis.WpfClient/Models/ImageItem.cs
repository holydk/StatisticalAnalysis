using MaterialDesignThemes.Wpf;

namespace StatisticalAnalysis.WpfClient.Models
{
    public class ImageItem : TabItem, IImageItem
    {
        public PackIconKind IconKind { get; }

        public ImageItem(string title, PackIconKind iconKind)
            : base(title)
            
        {
            IconKind = iconKind;
        }
    }
}
