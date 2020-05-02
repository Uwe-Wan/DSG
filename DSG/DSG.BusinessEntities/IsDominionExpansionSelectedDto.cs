namespace DSG.BusinessEntities
{
    public class IsDominionExpansionSelectedDto
    {
        public bool IsSelected { get; set; }

        public DominionExpansion DominionExpansion { get; set; }

        public IsDominionExpansionSelectedDto(DominionExpansion expansion)
        {
            DominionExpansion = expansion;
            IsSelected = true;
        }
    }
}
