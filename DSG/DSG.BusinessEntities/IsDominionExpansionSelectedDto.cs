using DSG.Common;

namespace DSG.BusinessEntities
{
    public class IsDominionExpansionSelectedDto : Notifier
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

        public DominionExpansion DominionExpansion { get; set; }

        public IsDominionExpansionSelectedDto(DominionExpansion expansion)
        {
            DominionExpansion = expansion;
            IsSelected = true;
        }
    }
}
