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

            _pagesViewModels = new IPageViewModel[]
            {
                hypTestingVm
            };

            _navigationItems = new INavigationItem[]
            {
                new NavigationItem(hypTestingVm.Title, typeof(HypothesisTestingView), MaterialDesignThemes.Wpf.PackIconKind.FormatListChecks)
            };

            _informationLinks = new ILink[]
            {
                new Link("Википедия", "", MaterialDesignThemes.Wpf.PackIconKind.Wikipedia)
            };
        }
    }
}
