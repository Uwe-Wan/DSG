﻿using DSG.BusinessEntities.CardAttributes;
using DSG.BusinessEntities.CardSubTypes;
using DSG.BusinessEntities.CardTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSG.BusinessEntities
{
    [Table("Card")]
    public class Card
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index("UQX_Card_Name_DominionExpansionId", 1, IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Cost")]
        public int CostId { get; set; }
        [Required]
        public Cost Cost { get; set; }

        [Required]
        [ForeignKey("DominionExpansion")]
        [Index("UQX_Card_Name_DominionExpansionId", 2, IsUnique = true)]
        public int DominionExpansionId { get; set; }
        [Required]
        public DominionExpansion DominionExpansion { get; set; }

        public List<CardTypeToCard> CardTypeToCards { get; set; }

        public List<CardSubTypeToCard> CardSubTypeToCards { get; set; }

        public List<CardAttribute> CardArtifacts { get; set; }
    }
}
