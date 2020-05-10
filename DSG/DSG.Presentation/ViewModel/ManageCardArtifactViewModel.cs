using DSG.BusinessComponents.CardAttributes;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Common.Extensions;
using DSG.Presentation.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel
{
    public class ManageCardArtifactViewModel : IViewModel
    {
        private ICardAttributeBc _cardArtifactBc;

        public ICardAttributeBc CardArtifactBc
        {
            get { return _cardArtifactBc; }
            set { _cardArtifactBc = value; }
        }

        public ObservableCollection<CardArtifact> CardArtifacts { get; set; }

        public CardArtifact CardArtifactToInsert { get; set; }

        public ObservableCollection<AdditionalCard> AdditionalCards { get; set; }

        public List<string> AvailableTypesOfAdditionalCard = Enum.GetValues(typeof(TypeOfAdditionalCard))
                    .OfType<TypeOfAdditionalCard>()
                    .Select(x => x.ToString())
                    .ToList();



        public ManageCardArtifactViewModel()
        {
            AddArtifactCommand = new RelayCommand(c => AddArtifact());
            CardArtifacts = new ObservableCollection<CardArtifact>();
            AdditionalCards = new ObservableCollection<AdditionalCard>();
        }

        public ICommand AddArtifactCommand { get; private set; }

        public async Task OnPageLoadedAsync(params object[] data)
        {
            CardArtifacts.Clear();
            AdditionalCards.Clear();

            IEnumerable<DominionExpansion> expansions = data[0] as IEnumerable<DominionExpansion>;
            List<CardArtifact> artifacts = expansions.SelectMany(expansion => expansion.ContainedCards).SelectMany(card => card.CardArtifacts).ToList();
            IEnumerable<AdditionalCard> additionalCards = artifacts.Select(artifact => artifact.AdditionalCard);

            CardArtifacts.AddRange(artifacts);
            AdditionalCards.AddRange(additionalCards);
        }

        internal void AddArtifact()
        {
            CardArtifactBc.InsertAttribute(CardArtifactToInsert);
            CardArtifacts.Add(CardArtifactToInsert);
        }
    }
}
