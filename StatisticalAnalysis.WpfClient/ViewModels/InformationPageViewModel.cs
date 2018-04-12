using System;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class InformationPageViewModel : PageViewModel, IInformationPageViewModel
    {
        public string SubTitle { get; }

        public InformationPageViewModel(string title)
            : base(title)
        {

        }
    }
}
