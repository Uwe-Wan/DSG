using System.Collections.Generic;

namespace DSG.BusinessComponents.Probabilities
{
    public interface IShuffleListBc<TEntity>
    {
        List<TEntity> ReturnGivenNumberOfDistinctRandomElementsFromList(List<TEntity> listToReturnFrom, int numberOfElements);
    }
}
