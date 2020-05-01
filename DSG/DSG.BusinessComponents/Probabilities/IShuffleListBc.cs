using System.Collections.Generic;

namespace DSG.BusinessComponents.Probabilities
{
    public interface IShuffleListBc<TEntity>
    {
        List<TEntity> ReturnGivenNumberOfRandomElementsFromList(List<TEntity> listToReturnFrom, int numberOfElements);
    }
}
