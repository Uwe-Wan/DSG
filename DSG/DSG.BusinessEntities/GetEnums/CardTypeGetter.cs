using DSG.BusinessEntities.CardTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessEntities.GetEnums
{
    public static class CardTypeGetter
    {
        public static List<CardTypeEnum> GetEnum()
        {
            IEnumerable<CardTypeEnum> enums = Enum.GetValues(typeof(CardTypeEnum)) as IEnumerable<CardTypeEnum>;
            return enums.ToList();
        }
    }
}
