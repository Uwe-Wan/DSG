using DSG.Common;
using System.ComponentModel;

namespace DSG.BusinessEntities.CardArtifacts
{
    public class AdditionalCard : Notifier
    {
        private int? _minCost;

        public int Id { get; set; }

        public bool AlreadyIncludedCard { get; set; }

        public int? MaxCosts { get; set; }

        public int? MinCosts
        {
            get { return _minCost; }
            set
            {
                _minCost = value;
                OnPropertyChanged(nameof(MinCosts));
            }
        }

        public AdditionalCard()
        {
        }

        public AdditionalCard(int? minCost, int? maxCost, TypeOfAdditionalCard typeOfAdditionalCard)
        {
            MinCosts = minCost;
            MaxCosts = maxCost;
            AlreadyIncludedCard = typeOfAdditionalCard == TypeOfAdditionalCard.Existing;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            AdditionalCard additionalCard = (AdditionalCard)obj;
            if (additionalCard.MaxCosts == this.MaxCosts && additionalCard.MinCosts == this.MinCosts)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            if (MaxCosts.HasValue)
            {
                hashCode = (hashCode << 2) ^ MaxCosts.Value;
            }
            if (MinCosts.HasValue)
            {
                hashCode = (base.GetHashCode() << 2) ^ MinCosts.Value;
            }
            return hashCode;
        }
    }
}
