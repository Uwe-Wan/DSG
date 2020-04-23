using DSG.BusinessEntities;
using DSG.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public string ExpansionName { get; set; }

        public ObservableCollection<Card> ContainedCards { get; set; }

        public DominionExpansion DominionExpansion { get; set; }
    }
}
