using DSG.Common;

namespace DSG.BusinessEntities.CardArtifacts
{
    public class IsCardArtifactSelectedDto : Notifier
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public string ArtifactName { get; set; }

        public CardArtifact Artifact { get; set; }
    }
}
