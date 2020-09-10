using DSG.Common;
using DSG.Common.Provider;
using System.Collections.Generic;

namespace DSG.BusinessComponents.Probabilities
{
    public class ShuffleListBc<TEntity> : IShuffleListBc<TEntity>
    {
        private IRandomProvider _random;

        public IRandomProvider Random
        {
            get
            {
                Check.RequireInjected(Random, nameof(Random), nameof(ShuffleListBc<TEntity>));
                return _random;
            }
            set
            {
                _random = value;
            }
        }

        public List<TEntity> ReturnGivenNumberOfRandomElementsFromList(List<TEntity> listToReturnFrom, int numberOfElements)
        {
            List<TEntity> chosenEntries = new List<TEntity>();

            for (int i = 0; i < numberOfElements; i++)
            {
                int randomListEntryToReturn = Random.GetRandomIntegerByUpperBoarder(listToReturnFrom.Count);

                TEntity selectedElement = listToReturnFrom[randomListEntryToReturn];
                chosenEntries.Add(selectedElement);

                listToReturnFrom.RemoveAt(randomListEntryToReturn);
            }

            return chosenEntries;
        }
    }
}
