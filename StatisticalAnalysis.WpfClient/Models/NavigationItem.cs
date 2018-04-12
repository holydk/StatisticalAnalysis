using MaterialDesignThemes.Wpf;
using System;

namespace StatisticalAnalysis.WpfClient.Models
{
    public class NavigationItem : TabItem, INavigationItem
    {
        public Type ViewType { get; }

        public NavigationItem(string title, Type viewType, PackIconKind iconKind)
            : base(title, iconKind)
        {
            ViewType = viewType;
        }
    }
}
