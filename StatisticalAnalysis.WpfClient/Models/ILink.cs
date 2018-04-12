namespace StatisticalAnalysis.WpfClient.Models
{
    public interface ILink : ITabItem
    {
        string Adress { get; }

        string ToolTip { get; }
    }
}
