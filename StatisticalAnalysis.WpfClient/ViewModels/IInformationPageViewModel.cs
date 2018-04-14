using StatisticalAnalysis.WpfClient.Models;
using System.Collections.Generic;
using System.Windows.Input;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public interface IInformationPageViewModel : IPageViewModel
    {
        IEnumerable<ILink> InformationLinks { get; }

        IEnumerable<IInformationItem> InformationItems { get; }

        ICommand OpenLinkCommand { get; }
    }
}
