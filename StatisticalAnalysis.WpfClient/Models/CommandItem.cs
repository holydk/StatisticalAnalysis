using MaterialDesignThemes.Wpf;
using System.Windows.Input;

namespace StatisticalAnalysis.WpfClient.Models
{
    public class CommandItem : ImageItem, ICommandItem
    {
        public ICommand Command { get; }

        public CommandItem(string title, PackIconKind iconKind, ICommand command)
            : base(title, iconKind)
        {
            Command = command;
        }
    }
}
