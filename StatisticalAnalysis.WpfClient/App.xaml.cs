using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.ViewModels;
using StatisticalAnalysis.WpfClient.Views;
using System.Windows;

namespace StatisticalAnalysis.WpfClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var navigation = new Navigation();
            var mainViewModel = new MainViewModel(navigation, "Math Stats");

            MainWindow = new MainWindow(mainViewModel);
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
