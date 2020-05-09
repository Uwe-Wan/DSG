namespace DSG.BusinessEntities.CardAttributes
{
    public class AdditionalCard
    {
        public int Id { get; set; }

        public bool AlreadyIncludedCard { get; set; }

        public int? MaxCosts { get; set; }

        public int? MinCosts { get; set; }
    }
}
