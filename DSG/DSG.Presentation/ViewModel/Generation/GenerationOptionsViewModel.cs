using DSG.BusinessEntities;
using DSG.Common.Extensions;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

[assembly: InternalsVisibleTo("DSG.Presentation.Test")]

namespace DSG.Presentation.ViewModel.Generation
{
    public class GenerationOptionsViewModel : IViewModel
    {
        private IUiService _uiService;
        private INaviService _naviService;

        public IUiService UiService
        {
            get { return _uiService; }
            set { _uiService = value; }
        }

        public INaviService NaviService
        {
            get { return _naviService; }
            set { _naviService = value; }
        }

        public List<IsDominionExpansionSelectedDto> IsDominionExpansionSelectedDtos { get; set; }

        public GenerationOptionsViewModel()
        {
            GenerateSetCommand = new RelayCommand(c => GenerateSet());

            IsDominionExpansionSelectedDtos = new List<IsDominionExpansionSelectedDto>();
        }

        public ICommand GenerateSetCommand { get; private set; }

        public async Task OnPageLoadedAsync(params object[] data)
        {
            IEnumerable<DominionExpansion> expansionData = data[0] as IEnumerable<DominionExpansion>;
            List<DominionExpansion> expansions = expansionData.ToList();

            IsDominionExpansionSelectedDtos = expansions.Select(expansion => new IsDominionExpansionSelectedDto(expansion)).ToList();
        }

        internal void GenerateSet()
        {
            int availableSupplyCards = IsDominionExpansionSelectedDtos.SelectMany(dto => dto.DominionExpansion.ContainedCards).Count();

            if (availableSupplyCards < 10)
            {
                string message = string.Join(" ", "There are only", availableSupplyCards, "cards available in the selected Sets. " +
                    "A minimum of 10 is needed to generate a set.");
                string caption = "Not enough Cards!";
                UiService.ShowErrorMessage(message, caption);
                return;
            }

            NavigateTo(NavigationDestination.GeneratedSet);
        }

        private void NavigateTo(NavigationDestination destination)
        {
            List<DominionExpansion> selectedExpansions = IsDominionExpansionSelectedDtos
                .Where(dto => dto.IsSelected == true)
                .Select(dto => dto.DominionExpansion)
                .ToList();
            NaviService.NavigateToAsync(destination, selectedExpansions);
        }
    }
}
