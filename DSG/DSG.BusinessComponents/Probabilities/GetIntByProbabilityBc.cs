using DSG.Common;
using DSG.Common.Exceptions;
using DSG.Common.Provider;
using System.Linq;

namespace DSG.BusinessComponents.Probabilities
{
    public class GetIntByProbabilityBc : IGetIntByProbabilityBc
    {
        private IRandomProvider _randomProvider;

        public IRandomProvider Random
        {
            get
            {
                Check.RequireInjected(_randomProvider, nameof(Random), nameof(GetIntByProbabilityBc));
                return _randomProvider;
            }
            set
            {
                _randomProvider = value;
            }
        }

        /// <summary>
        /// Returns the parameter position of the parameter with the likelihood of its value.
        /// </summary>
        public int GetRandomIntInBetweenZeroAndInputParameterCount(int probabilityOne, params int[] probabilities)
        {
            if (probabilityOne + probabilities.Sum() > 100)
            {
                throw new ProbabilitiesTooHighException("The sum of all entered probabilities is higher than 100%");
            }

            int random = Random.GetRandomIntegerByUpperBoarder(100);

            // if the random number in between 0 and 100 is smaller than the percentage for 1, return 1. Assume probabilityOne is 25.
            //Random is smaller than 25 with 25% chance. So this return 1 with 25% chance.
            if (probabilityOne > random)
            {
                return 1;
            }

            int processedProbabilities = probabilityOne;


            for (int i = 0; i < probabilities.Count(); i++)
            {
                if (processedProbabilities + probabilities[i] > random)
                {
                    return i + 2;
                }

                processedProbabilities += probabilities[i];
            }

            return 0;
        }
    }
}
