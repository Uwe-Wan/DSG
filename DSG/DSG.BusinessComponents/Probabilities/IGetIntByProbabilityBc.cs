namespace DSG.BusinessComponents.Probabilities
{
    public interface IGetIntByProbabilityBc
    {
        /// <summary>
        /// Returns the parameter position of the parameter with the likelihood of its value.
        /// </summary>
        int GetRandomIntInBetweenZeroAndInputParameterCount(int probabilityOne, params int[] probabilities);
    }
}
