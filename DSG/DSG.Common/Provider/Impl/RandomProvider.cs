using System;

namespace DSG.Common.Provider.Impl
{
    public class RandomProvider : IRandomProvider
    {
        public int GetRandomInteger()
        {
            Random random = new Random();
            return random.Next();
        }

        public int GetRandomIntegerByUpperBoarder(int upperBoarder)
        {
            Random random = new Random();
            return random.Next(upperBoarder);
        }

        public int GetRandomIntegerByLowerAndUpperBoarder(int lowerBoarder, int upperBoarder)
        {
            Random random = new Random();
            return random.Next(lowerBoarder, upperBoarder);
        }
    }
}
