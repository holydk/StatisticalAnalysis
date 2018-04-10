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
            MainWindow = new MainWindow();

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
