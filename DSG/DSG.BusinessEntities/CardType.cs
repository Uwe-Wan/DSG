using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.BusinessEntities
{
    [Table("CardType")]
    public class CardType
    {
        public CardTypeEnum Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<Card> Cards { get; set; }
    }
}
