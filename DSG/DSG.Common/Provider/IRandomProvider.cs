using System;

namespace DSG.Common.Provider
{
    public interface IRandomProvider
    {
        /// <summary>
        /// Gets a random integer in between 0 and upper boarder (upper boarder is excluded).
        /// </summary>
        int GetRandomIntegerByUpperBoarder(int upperBoarder);
    }
}
