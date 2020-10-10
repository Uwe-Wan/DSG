using System;

namespace DSG.Common.Provider.Impl
{
    public class RandomProvider : IRandomProvider
    {
        public Random Random { get; set; }

        public RandomProvider()
        {
            Random = new Random();
        }

        public int GetRandomIntegerByUpperBoarder(int upperBoarder)
        {
            return Random.Next(upperBoarder);
        }
    }
}
