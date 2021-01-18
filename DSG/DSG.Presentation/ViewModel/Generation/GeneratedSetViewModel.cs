using DSG.BusinessComponents.Generation;
using DSG.BusinessEntities;
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
        private string _containsSheltersOrColony;

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

        public string ContainsSheltersOrColony
        {
            get
            {
                return _containsSheltersOrColony == string.Empty ? string.Empty : "This set is also played with " + _containsSheltersOrColony;
            }
            set
            {
                _containsSheltersOrColony = value;
                OnPropertyChanged(nameof(ContainsSheltersOrColony));
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

        private void LoadData(object generationParameterData)
        {
            GenerationParameterDto generationParameter = generationParameterData as GenerationParameterDto;
            GeneratedSetDto generatedSetDto = SetGeneratorBc.GenerateSet(generationParameter);

            HandleSupplyStuff(generatedSetDto);
            HandleNonSupplyStuff(generatedSetDto);
            HandleCardIndependentStuff(generatedSetDto);
        }

        private void HandleCardIndependentStuff(GeneratedSetDto generatedSetDto)
        {
            List<string> cardIndependentStuff = new List<string>();
            if (generatedSetDto.HasPlatinumAndColony)
            {
                cardIndependentStuff.Add("Colony and Platinum");
            }

            if (generatedSetDto.HasShelters)
            {
                cardIndependentStuff.Add("Shelters");
            }
            ContainsSheltersOrColony = string.Join(", ", cardIndependentStuff);
        }

        private void HandleNonSupplyStuff(GeneratedSetDto generatedSetDto)
        {
            NonSupplyStuff = generatedSetDto.NonSupplyCards.Select(card => new CardAndArtifactViewEntity(card)).ToList();
            NonSupplyStuff.AddRange(generatedSetDto.ArtifactsWithoutAdditionalCards.Select(artifact => new CardAndArtifactViewEntity(artifact)));
            NonSupplyStuff = NonSupplyStuff
                .OrderBy(card => card.Set)
                .ThenBy(card => card.Cost)
                .ThenBy(card => card.BelongsTo)
                .ToList();
        }

        private void HandleSupplyStuff(GeneratedSetDto generatedSetDto)
        {
            SupplyCards = generatedSetDto.SupplyCardsWithoutAdditional.Select(card => new CardAndArtifactViewEntity(card)).ToList();
            SupplyCards.AddRange(generatedSetDto.GeneratedAdditionalCards.Select(gac => new CardAndArtifactViewEntity(gac)));
            SupplyCards.AddRange(generatedSetDto.GeneratedExistingAdditionalCards.Select(gac => new CardAndArtifactViewEntity(gac)));
            SupplyCards = SupplyCards
                .OrderBy(card => card.Set)
                .ThenBy(card => card.BelongsTo)
                .ThenBy(card => card.Cost)
                .ToList();
        }

        public override async Task NavigateToAsync(NavigationDestination destination)
        {
            await base.NavigateToAsync(destination);
            await Task.Run(ClearData);
        }

        private void ClearData()
        {
            SupplyCards = null;
            ContainsSheltersOrColony = string.Empty;
        }
    }
}
