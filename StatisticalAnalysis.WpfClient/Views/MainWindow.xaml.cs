using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using StatisticalAnalysis.WpfClient.Commands;
using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView<MainViewModel>
    {
        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public static bool GetIsBusy(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsBusyProperty);
        }

        public static void SetIsBusy(DependencyObject obj, bool value)
        {
            obj.SetValue(IsBusyProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.RegisterAttached("IsBusy", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        private int _currentNavIndex;

        public MainViewModel ViewModel
        {
            get => DataContext as MainViewModel;
            set => DataContext = value;
        }

        public MainWindow()
        {
            SetIsBusy(this, true);

            InitializeComponent();

            MinimizeCommand = new RelayCommand((sender) => WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand((sender) => WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand((sender) => Close());

            ViewModel = new MainViewModel();

            RoutedEventHandler loaded = null;
            loaded = async (obj, e) =>
            {
                Loaded -= loaded;

                var PART_Header = (Border)Template.FindName("PART_Header", this);

                if (PART_Header != null)
                    PART_Header.Background = (SolidColorBrush)Application.Current.Resources["MaterialDesignPaper"];

                var mainContent = ((ContentPresenter)Template.FindName("mainWindowContent", this)).Content as Grid;              

                if (mainContent != null)
                {
                    await Task.Run(() => Thread.Sleep(500)).ContinueWith(async (t) =>
                    {
                        var waitPageIcon = (TransitioningContent)mainContent.Children[0];

                        if (waitPageIcon != null)
                            waitPageIcon.Visibility = Visibility.Visible;

                        await Task.Delay(500);
                    }, TaskScheduler.FromCurrentSynchronizationContext())
                    .ContinueWith((t) =>
                    {
                        var waitPageTitle = (TransitioningContent)mainContent.Children[1];

                        if (waitPageTitle != null)
                            waitPageTitle.Visibility = Visibility.Visible;

                    }, TaskScheduler.FromCurrentSynchronizationContext());  
                }              

                await Task.Delay(5000);

                SetIsBusy(this, false);

                await ViewModel.Navigation.GoToAsync(() => new TestView()
                {
                    DataContext = new TestViewModel()
                });

                var listBox = (ListBox)Template.FindName("DemoItemsListBox", this);

                // maintTitle заголовок главной страницы
                // необходимо знать ее Тип чтобы потом 
                // вернутся на эту страницу с дочерней страницы
                // Пример : МайнТайтл > Тайтл1 > Тайтл2 и тд
                // Если мы находимся на дочерней странице Тайтл2, то 
                // при нажатии на МайнТайтл должны перейти
                // на главную страницу при этом Тайтл1 и Тайтл2
                // удаляются из навиг. меню.
                mainTitle.Tag = ViewModel.Navigation.Content.GetType();

                ViewModel.Navigation.Navigated += (_obj, _e) =>
                {
                    if (_e.NavigationState == NavigationState.GoForward)
                    {
                        var chevronRight = new PackIcon()
                        {
                            Kind = PackIconKind.ChevronRight,
                            Width = 20,
                            Foreground = (SolidColorBrush)Application.Current.Resources["PrimaryHueMidForegroundBrush"]
                        };

                        var uiContainer = new InlineUIContainer(chevronRight)
                        {
                            BaselineAlignment = BaselineAlignment.Center
                        };

                        var navDataContext = ViewModel.Navigation.Content.DataContext;
                        var pageTitle = new Run((navDataContext as IPageViewModel).Title)
                        {
                            Tag = ViewModel.Navigation.Content.GetType()
                        };

                        navTextBox.Inlines.AddRange(new Inline[]
                        {
                            uiContainer,
                            pageTitle
                        });
                    }
                    else if (_e.NavigationState == NavigationState.GoBack)
                    {
                        while (navTextBox.Inlines.Count - 1 > _currentNavIndex)
                        {
                            navTextBox.Inlines.Remove(navTextBox.Inlines.LastInline);
                        }
                    }
                };
            };

            Loaded += loaded;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            var containerBorder = (Border)Template.FindName("PART_Container", this);

            if (containerBorder == null) return;

            if (WindowState == WindowState.Maximized)
            {
                // Make sure window doesn't overlap with the taskbar.

                //var screen = System.Windows.Forms.Screen.FromHandle(handle);
                //if (screen.Primary)
                {
                    containerBorder.Padding = new Thickness(
                        SystemParameters.WorkArea.Left + 7,
                        SystemParameters.WorkArea.Top + 7,
                        (SystemParameters.PrimaryScreenWidth - SystemParameters.WorkArea.Right) + 5,
                        (SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Bottom) - 40);
                }
            }
            else
            {
                containerBorder.Padding = new Thickness(0, 0, 0, 0);
            }

            base.OnStateChanged(e);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.Navigation.GoToAsync(() => new TestView1() { DataContext = new TestViewModel() });
        }

        private async void Run_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Run run)
            {
                _currentNavIndex = navTextBox.Inlines.ToList().IndexOf(run);

                await ViewModel.Navigation.GoBackAsync(run.Tag as Type);
            }
        }
    }
}
