using DSG.Common;

namespace DSG.BusinessEntities
{
    public class IsSelectedAndWeightedExpansionDto : Notifier
    {
        private bool _isSelected;
        private int _weight;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                UpdateWeightBySelection(value);
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public int Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public DominionExpansion DominionExpansion { get; set; }

        public IsSelectedAndWeightedExpansionDto(DominionExpansion expansion, int weight = 1)
        {
            DominionExpansion = expansion;
            IsSelected = true;
            Weight = weight;
        }

        internal void UpdateWeightBySelection(bool isSelected)
        {
            Weight = isSelected ? 1 : 0;
        }
    }
}
