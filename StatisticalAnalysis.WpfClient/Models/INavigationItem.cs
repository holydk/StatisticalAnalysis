using System;

namespace StatisticalAnalysis.WpfClient.Models
{
    public interface INavigationItem : IImageItem
    {
        Type ViewType { get; }
    }
}
