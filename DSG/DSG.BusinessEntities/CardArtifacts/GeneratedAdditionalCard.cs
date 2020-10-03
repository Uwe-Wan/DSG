namespace DSG.BusinessEntities.CardArtifacts
{
    public class GeneratedAdditionalCard
    {
        public Card AdditionalCard { get; set; }

        public Card Parent { get; set; }

        public GeneratedAdditionalCard(Card additionalCard, Card parent)
        {
            AdditionalCard = additionalCard;
            Parent = parent;
        }
    }
}
