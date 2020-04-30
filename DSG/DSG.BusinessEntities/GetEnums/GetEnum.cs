using DSG.BusinessEntities.CardSubTypes;
using DSG.BusinessEntities.CardTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSG.BusinessEntities.GetEnums
{
    public static class GetEnum
    {
        public static List<CardTypeEnum> CardType()
        {
            IEnumerable<CardTypeEnum> enums = Enum.GetValues(typeof(CardTypeEnum)) as IEnumerable<CardTypeEnum>;
            return enums.ToList();
        }

        public static List<CardSubTypeEnum> CardSubType()
        {
            IEnumerable<CardSubTypeEnum> enums = Enum.GetValues(typeof(CardSubTypeEnum)) as IEnumerable<CardSubTypeEnum>;
            return enums.ToList();
        }
    }
}
