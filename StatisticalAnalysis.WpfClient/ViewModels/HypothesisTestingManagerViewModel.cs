using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.Views;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class HypothesisTestingManagerViewModel : NavigationPageViewModel, IPageViewModel
    {
        public HypothesisTestingManagerViewModel(INavigation navigation)
            : base(navigation, "Проверка гипотез")
        {
            var tTypeDistrVm = new TTypeDistributionViewModel();
            navigation.Add(() => new TTypeDistributionView(tTypeDistrVm));

            _pagesViewModels = new IPageViewModel[]
            {
                tTypeDistrVm
            };

            _navigationItems = new INavigationItem[]
            {
                new NavigationItem(tTypeDistrVm.Title, typeof(TTypeDistributionView), MaterialDesignThemes.Wpf.PackIconKind.FormatListChecks)
            };

            _informationItems = new IInformationItem[]
            {
                new InformationItem("Статистическая гипотеза", "Это определённое предположение о свойствах случайных величин на основе наблюдаемой выборки данных."),
                new InformationItem("Проверка статистической гипотезы", "Это процесс принятия решения о том, противоречит ли рассматриваемая статистическая гипотеза наблюдаемой выборке данных."),
                new InformationItem("Этапы проверки статистической гипотезы", "1. Формулировка нулевой и альтернативной гипотез.\n2. Выбор соответствующего статистического теста.\n3. Выбор требуемого уровня значимости (α = 0.05, 0.01, 0.025, …).\n4. Вычисление статистики критерия по тесту.\n5. Сравнение эмпирического значения критерия с критическим значением по тесту.\n6. Принятие решения: если вычисленное значение больше, чем критическое, то нулевая гипотеза отклоняется.")
            };

            _informationLinks = new ILink[]
            {
                new Link("Википедия", "https://ru.wikipedia.org/wiki/%D0%9F%D1%80%D0%BE%D0%B2%D0%B5%D1%80%D0%BA%D0%B0_%D1%81%D1%82%D0%B0%D1%82%D0%B8%D1%81%D1%82%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B8%D1%85_%D0%B3%D0%B8%D0%BF%D0%BE%D1%82%D0%B5%D0%B7", MaterialDesignThemes.Wpf.PackIconKind.Wikipedia)
            };
        }
    }
}
