using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.Views;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class StatsAnalysisManagerViewModel : NavigationPageViewModel
    {
        public StatsAnalysisManagerViewModel(INavigation navigation)
            : base(navigation, "Статистический анализ")
        {
            _informationItems = new IInformationItem[]
            {
                new InformationItem("Cтатистический анализ", "Статистика — отрасль знаний, наука, в которой излагаются общие вопросы сбора, измерения, мониторинга и анализа массовых статистических (количественных или качественных) данных; изучение количественной стороны массовых общественных явлений в числовой форме."),
                new InformationItem("Что делать дальше?", "Выберите тематику, необходимую для дальнейших исследований."),
                new InformationItem("Долго искать необходимую тему?", "Воспользуйтесь кнопкой поиска.")
            };

            var hypTestingVm = new HypothesisTestingManagerViewModel(navigation);
            Navigation.Add(() => new HypothesisTestingView(hypTestingVm));

            var corRegAnalysisVm = new CorRegAnalysisViewModel();
            Navigation.Add(() => new CorRegAnalysisView(corRegAnalysisVm));

            var timeSerAnalysisVm = new TimeSeriesAnalysisViewModel();
            Navigation.Add(() => new TimeSeriesAnalysisView(timeSerAnalysisVm));

            var forecastingTimeSerVm = new ForecastingTimeSeriesViewModel();
            Navigation.Add(() => new ForecastingTimeSeriesView(forecastingTimeSerVm));

            _pagesViewModels = new IPageViewModel[]
            {
                hypTestingVm, corRegAnalysisVm, timeSerAnalysisVm
            };

            _navigationItems = new INavigationItem[]
            {
                new NavigationItem(hypTestingVm.Title, typeof(HypothesisTestingView), MaterialDesignThemes.Wpf.PackIconKind.FormatListChecks),
                new NavigationItem(corRegAnalysisVm.Title, typeof(CorRegAnalysisView), MaterialDesignThemes.Wpf.PackIconKind.ChartArc),
                new NavigationItem(timeSerAnalysisVm.Title, typeof(TimeSeriesAnalysisView), MaterialDesignThemes.Wpf.PackIconKind.ChartLine),
                new NavigationItem(forecastingTimeSerVm.Title, typeof(ForecastingTimeSeriesView), MaterialDesignThemes.Wpf.PackIconKind.ChartTimeline)
            };

            _informationLinks = new ILink[]
            {
                new Link("Википедия", "https://ru.wikipedia.org/wiki/%D0%A1%D1%82%D0%B0%D1%82%D0%B8%D1%81%D1%82%D0%B8%D0%BA%D0%B0", MaterialDesignThemes.Wpf.PackIconKind.Wikipedia)
            };
        }
    }
}
