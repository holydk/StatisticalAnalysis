using MaterialDesignThemes.Wpf.Transitions;
using StatisticalAnalysis.WpfClient.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace StatisticalAnalysis.WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для TTypeDistributionView.xaml
    /// </summary>
    public partial class TTypeDistributionView : UserControl, IView
    {
        public IPageViewModel ViewModel
        {
            get => DataContext as IPageViewModel;
            set => DataContext = value;
        }
    
        public TTypeDistributionView(IPageViewModel viewModel)
        {
            ViewModel = viewModel;

            InitializeComponent();

            if (viewModel is TTypeDistributionViewModel distrVM)
            {
                distrVM.IsBusy = false;
                distrVM.IsResult = false;
                distrVM.VariationData = null;
                distrVM.SelectedSignificanceLevel = null;
                distrVM.SelectedDistributionType = null;
                distrVM.SelectedDistributionSeriesInputType = null;

                PropertyChangedEventHandler eventHandler = null;
                eventHandler = async (obj, e) =>
                {
                    if (e.PropertyName == "IsBusy" && distrVM.IsBusy)
                    {
                        if (!distrVM.IsResult)
                        {
                            BeginAnimation(PART_inputParams, new Thickness(0, -PART_inputParams.ActualHeight, 0, 0), 0, null,
                                () => PART_inputParams.Visibility = Visibility.Collapsed);
                            BeginAnimation(PART_inputData, new Thickness(-PART_inputData.ActualWidth, 0, 0, 0), 0, 300); 
                        }
                        else
                        {
                            if (PART_resultContentControl.Content is Grid grid && distrVM.IsResult && grid.Children.Count > 0)
                            {
                                if (grid.Children[1] is ContentControl contentControl &&
                                    contentControl.Content is ScrollViewer scrollViewer)
                                {
                                    scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                                }
                            }
                        }
                    }
                    else if (e.PropertyName == "IsResult")
                    {
                        if (!distrVM.IsResult)
                        {
                            PART_inputParams.Visibility = Visibility.Visible;
                            BeginAnimation(PART_inputParams, new Thickness(0), 1);

                            if (distrVM.VariationData != null)
                            {
                                BeginAnimation(PART_inputData, new Thickness(0), 1, 300);
                            } 
                        }
                        else
                        {
                            if (PART_resultContentControl.Content is Grid grid && !distrVM.IsBusy && grid.Children.Count > 0)
                            {
                                if (grid.Children[1] is ContentControl contentControl &&
                                    contentControl.Content is ScrollViewer scrollViewer)
                                {
                                    await Task.Delay(400);
                                    scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                                }
                            }
                        }
                    }
                };

                distrVM.PropertyChanged -= eventHandler;
                distrVM.PropertyChanged += eventHandler;
            }
        }

        private static void BeginAnimation(FrameworkElement element, Thickness toThickness, double toOpacity, int? beginTime = null, Action onComplited = null)
        {
            var thicknessAnimation = new ThicknessAnimation(toThickness, new TimeSpan(0, 0, 0, 0, 400))
            {
                EasingFunction = new SineEase()
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };
            thicknessAnimation.Completed += (sender, args) =>
            {
                thicknessAnimation.BeginAnimation(MarginProperty, null);
                onComplited?.Invoke();
            };

            var opacityAnimation = new DoubleAnimation(toOpacity, new TimeSpan(0, 0, 0, 0, 400))
            {
                EasingFunction = new SineEase()
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };
            opacityAnimation.Completed += (sender, args) =>
            {
                opacityAnimation.BeginAnimation(OpacityProperty, null);
            };

            Storyboard.SetTarget(thicknessAnimation, element);
            Storyboard.SetTarget(opacityAnimation, element);

            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath(MarginProperty));
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(OpacityProperty));

            var storyboard = new Storyboard();
            storyboard.Children.Add(thicknessAnimation);
            storyboard.Children.Add(opacityAnimation);

            if (beginTime.HasValue)
                storyboard.BeginTime = new TimeSpan(0, 0, 0, 0, beginTime.Value);

            storyboard.Begin();
        }
    }
}
