using DSG.Common;

namespace DSG.BusinessEntities.CardArtifacts
{
    public class AdditionalCard : Notifier
    {
        private int? _minCost;

        public int Id { get; set; }

        public bool AlreadyIncludedCard { get; set; }

        public int? MaxCosts { get; set; }

        public int? MinCosts { get; set; }

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
            if (additionalCard.MaxCosts == this.MaxCosts && 
                additionalCard.MinCosts == this.MinCosts &&
                additionalCard.AlreadyIncludedCard == this.AlreadyIncludedCard)
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
                hashCode = (GetHashCode() << 2) ^ MinCosts.Value;
            }
            hashCode = (GetHashCode() << 2) ^ AlreadyIncludedCard.GetHashCode();
            
            return hashCode;
        }
    }
}
