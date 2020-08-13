using DSG.BusinessEntities;
using DSG.BusinessEntities.CardArtifacts;
using DSG.Common.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DSG.Presentation.ViewEntity
{
    public class SelectedExpansionViewEntity
    {
        public SelectedExpansionViewEntity(DominionExpansion dominionExpansion)
        {
            ExpansionName = dominionExpansion.Name;

            ContainedCards = new ObservableCollection<Card>();
            ContainedCards.AddRange(dominionExpansion.ContainedCards);

            DominionExpansion = dominionExpansion;

            ContainedArtifacts = new ObservableCollection<CardArtifact>();
            ContainedArtifacts.AddRange(dominionExpansion.ContainedArtifacts);
        }

        public string ExpansionName { get; set; }

        public ObservableCollection<Card> ContainedCards { get; set; }

        public DominionExpansion DominionExpansion { get; set; }

        public ObservableCollection<CardArtifact> ContainedArtifacts { get; set; }
    }
}
