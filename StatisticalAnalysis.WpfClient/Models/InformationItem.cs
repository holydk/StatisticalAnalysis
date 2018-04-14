﻿using MaterialDesignThemes.Wpf;

namespace StatisticalAnalysis.WpfClient.Models
{
    public class InformationItem : TabItem, IInformationItem
    {
        public string Description { get; }

        public InformationItem(string title, string description, PackIconKind? iconKind = null)
            : base(title, iconKind ?? PackIconKind.Null)
        {
            Description = description;
        }
    }
}
