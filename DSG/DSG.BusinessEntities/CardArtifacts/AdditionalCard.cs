using DSG.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities.CardArtifacts
{
    public class AdditionalCard : Notifier
    {
        private int? _minCost;

        public int Id { get; set; }

        [Index("UQX_AdditionalCards_AlreadyIncludedCard_MaxCost_MinCost", 1, IsUnique = true)]
        public bool AlreadyIncludedCard { get; set; }

        [Index("UQX_AdditionalCards_AlreadyIncludedCard_MaxCost_MinCost", 2, IsUnique = true)]
        public int? MaxCost { get; set; }

        [Index("UQX_AdditionalCards_AlreadyIncludedCard_MaxCost_MinCost", 3, IsUnique = true)]
        public int? MinCost { get; set; }

        public AdditionalCard()
        {
        }

        public AdditionalCard(int? minCost, int? maxCost, TypeOfAdditionalCard typeOfAdditionalCard)
        {
            MinCost = minCost;
            MaxCost = maxCost;
            AlreadyIncludedCard = typeOfAdditionalCard == TypeOfAdditionalCard.Existing;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            AdditionalCard additionalCard = (AdditionalCard)obj;
            if (additionalCard.MaxCost == this.MaxCost && 
                additionalCard.MinCost == this.MinCost &&
                additionalCard.AlreadyIncludedCard == this.AlreadyIncludedCard)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            if (MaxCost.HasValue)
            {
                hashCode = (hashCode << 2) ^ MaxCost.Value;
            }
            if (MinCost.HasValue)
            {
                hashCode = (hashCode << 2) ^ MinCost.Value;
            }
            hashCode = (hashCode << 2) ^ AlreadyIncludedCard.GetHashCode();
            
            return hashCode;
        }
    }
}
