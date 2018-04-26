using System.Windows.Input;

namespace StatisticalAnalysis.WpfClient.Models
{
    public interface ICommandItem : IImageItem
    {
        ICommand Command { get; }
    }
}
