using System;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Models
{
    public class NavigationEventArgs : EventArgs
    {
        public NavigationState NavigationState { get; }
        public UserControl Content { get; }

        public NavigationEventArgs(NavigationState navigationState, UserControl content)
        {
            NavigationState = navigationState;
            Content = content;
        }
    }
}
