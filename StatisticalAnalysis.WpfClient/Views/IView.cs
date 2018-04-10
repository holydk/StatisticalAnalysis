namespace StatisticalAnalysis.WpfClient.Views
{
    public interface IView<T> where T : class
    {
        T ViewModel { get; set; }
    }
}
