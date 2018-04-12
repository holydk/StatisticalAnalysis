using System;

namespace StatisticalAnalysis.WpfClient.Models
{
    public interface INavigationItem : ITabItem
    {
        Type ViewType { get; }
    }
}
