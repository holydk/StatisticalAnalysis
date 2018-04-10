using StatisticalAnalysis.WpfClient.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для TestView1.xaml
    /// </summary>
    public partial class TestView1 : UserControl
    {
        public TestView1()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Parent is Control control && control.DataContext is INavigationViewModel navViewModel)
            {
                await navViewModel.Navigation.GoToAsync(() => new TestView2() { DataContext = new TestViewModel() });
            }
        }
    }
}
