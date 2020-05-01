namespace DSG.Common.Provider
{
    public interface IRandomProvider
    {
        /// <summary>
        /// Gets a random integer.
        /// </summary>
        int GetRandomInteger();

        /// <summary>
        /// Gets a random integer in between 0 and upper boarder (upper boarder is excluded).
        /// </summary>
        int GetRandomIntegerByUpperBoarder(int upperBoarder);

        /// <summary>
        /// Gets a random integer in between lower boarder and upper boarder (upper boarder is excluded, lower boarder included).
        /// </summary>
        int GetRandomIntegerByLowerAndUpperBoarder(int lowerBoarder, int upperBoarder);
    }
}
