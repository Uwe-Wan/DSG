using DSG.Common;

namespace DSG.BusinessEntities.CardSubTypes
{
    public class IsCardSubTypeSelectedDto : Notifier
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

        public CardSubTypeEnum CardSubType { get; set; }
    }
}
