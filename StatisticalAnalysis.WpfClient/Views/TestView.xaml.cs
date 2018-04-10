using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для TestView.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        public TestView()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Parent is Control control && control.DataContext is INavigationViewModel navViewModel)
            {
                await navViewModel.Navigation.GoToAsync(() => new TestView1() { DataContext = new TestViewModel() });
            }
        }
    }
}
