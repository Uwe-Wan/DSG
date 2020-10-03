using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Common;
using DSG.Presentation.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel.Generation
{
    public class GeneratedSetViewModel : AbstractViewModel, IViewModel
    {
        private ISetGeneratorBc _setGeneratorBc;
        private List<Card> _generatedSet;
        private List<CardArtifact> _artifactsOfGeneratedSet;

        public List<Card> GeneratedSet
        {
            get { return _generatedSet; }
            set
            {
                _generatedSet = value;
                ArtifactsOfGeneratedSet = value?.SelectMany(card => card.CardArtifactsToCard).Select(x => x.CardArtifact).ToList();
                OnPropertyChanged(nameof(GeneratedSet));
            }
        }

        public List<CardArtifact> ArtifactsOfGeneratedSet
        {
            get { return _artifactsOfGeneratedSet; }
            set
            {
                _artifactsOfGeneratedSet = value;
                OnPropertyChanged(nameof(ArtifactsOfGeneratedSet));
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
            GeneratedSet = generatedSetDto.SupplyCardsWithoutAdditional
                .Union(generatedSetDto.GeneratedAdditionalCards.Select(x => x.AdditionalCard))
                .Union(generatedSetDto.GeneratedExistingAdditionalCards.Select(x => x.AdditionalCard))
                .ToList();
        }

        public override async Task NavigateToAsync(NavigationDestination destination)
        {
            await base.NavigateToAsync(destination);
            await Task.Run(ClearData);
        }

        private void ClearData()
        {
            GeneratedSet = null;
        }
    }
}
