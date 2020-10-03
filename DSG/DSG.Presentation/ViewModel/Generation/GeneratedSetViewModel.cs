using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Common;
using DSG.Presentation.Services;
using DSG.Presentation.ViewEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel.Generation
{
    public class GeneratedSetViewModel : AbstractViewModel, IViewModel
    {
        private ISetGeneratorBc _setGeneratorBc;
        private List<CardAndArtifactViewEntity> _supplyCards;
        private List<CardAndArtifactViewEntity> _nonSupplyStuff;

        public List<CardAndArtifactViewEntity> SupplyCards
        {
            get { return _supplyCards; }
            set
            {
                _supplyCards = value;
                OnPropertyChanged(nameof(SupplyCards));
            }
        }

        public List<CardAndArtifactViewEntity> NonSupplyStuff
        {
            get { return _nonSupplyStuff; }
            set
            {
                _nonSupplyStuff = value;
                OnPropertyChanged(nameof(NonSupplyStuff));
            }
        }

        public ISetGeneratorBc SetGeneratorBc
        {
            get
            {
                Check.RequireInjected(_setGeneratorBc, nameof(SetGeneratorBc), nameof(GeneratedSetViewModel));
                return _setGeneratorBc;
            }
            set { _setGeneratorBc = value; }
        }


        public ICommand NavigateToGenerationOptionsCommand { get; private set; }

        public GeneratedSetViewModel()
        {
            NavigateToGenerationOptionsCommand = new RelayCommand(async c => await NavigateToAsync(NavigationDestination.GenerationOptions));
        }

        public override async Task OnPageLoadedAsync(params object[] data)
        {
            IsDataLoaded = false;
            await Task.Run(() => LoadData(data[0]));
            IsDataLoaded = true;
        }

        private void LoadData(object expansionsData)
        {
            List<DominionExpansion> expansions = expansionsData as List<DominionExpansion>;
            GeneratedSetDto generatedSetDto = SetGeneratorBc.GenerateSet(expansions);

            SupplyCards = generatedSetDto.SupplyCardsWithoutAdditional.Select(card => new CardAndArtifactViewEntity(card)).ToList();
            SupplyCards.AddRange(generatedSetDto.GeneratedAdditionalCards.Select(gac => new CardAndArtifactViewEntity(gac.AdditionalCard)));
            SupplyCards.AddRange(generatedSetDto.GeneratedExistingAdditionalCards.Select(gac => new CardAndArtifactViewEntity(gac.AdditionalCard)));
            SupplyCards = SupplyCards
                .OrderBy(card => card.Set)
                .ThenBy(card => card.BelongsTo)
                .ThenBy(card => card.Cost)
                .ToList();

            NonSupplyStuff = generatedSetDto.NonSupplyCards.Select(card => new CardAndArtifactViewEntity(card)).ToList();
            NonSupplyStuff.AddRange(generatedSetDto.ArtifactsWithoutAdditionalCards.Select(artifact => new CardAndArtifactViewEntity(artifact)));
        }

        public override async Task NavigateToAsync(NavigationDestination destination)
        {
            await base.NavigateToAsync(destination);
            await Task.Run(ClearData);
        }

        private void ClearData()
        {
            SupplyCards = null;
        }
    }
}
