using DSG.Common;

namespace DSG.BusinessEntities.CardTypes
{
    public class IsCardTypeSelectedDto : Notifier
    {
        private bool _isSelected;

        public CardTypeEnum CardType { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
