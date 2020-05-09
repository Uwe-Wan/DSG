using DSG.BusinessComponents.CardAttributes;
using DSG.BusinessEntities;
using DSG.BusinessEntities.CardAttributes;
using DSG.Common.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DSG.Presentation.ViewModel.Generation
{
    public class ManageCardArtifactViewModel
    {
        private ICardAttributeBc _cardAttributeBc;

        public ICardAttributeBc CardAttributeBc
        {
            get { return _cardAttributeBc; }
            set { _cardAttributeBc = value; }
        }

        public ObservableCollection<CardAttribute> CardArtifacts { get; set; }

        public CardAttribute CardArtifactToInsert { get; set; }

        public ManageCardArtifactViewModel()
        {
            AddArtifactCommand = new RelayCommand(c => AddArtifact());
            CardArtifacts = new ObservableCollection<CardAttribute>();
        }

        public ICommand AddArtifactCommand { get; private set; }

        public async Task OnPageLoadedAsync(params object[] data)
        {
            CardArtifacts.Clear();

            IEnumerable<DominionExpansion> expansions = data[0] as IEnumerable<DominionExpansion>;
            IEnumerable<CardAttribute> artifacts = expansions.SelectMany(x => x.ContainedCards).SelectMany(card => card.CardAttributes);

            CardArtifacts.AddRange(artifacts);
        }

        internal void AddArtifact()
        {
            CardAttributeBc.InsertAttribute(CardArtifactToInsert);
            CardArtifacts.Add(CardArtifactToInsert);
        }
    }
}
