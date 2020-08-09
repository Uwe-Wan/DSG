using DSG.BusinessEntities.CardArtifacts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities
{
    [Table("DominionExpansion")]
    public class DominionExpansion
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public List<Card> ContainedCards { get; set; }

        public List<CardArtifactToExpansion> ContainedArtifactsToExpansion { get; set; }

        public DominionExpansion()
        {
            ContainedCards = new List<Card>();
            ContainedArtifactsToExpansion = new List<CardArtifactToExpansion>();
        }
    }
}
