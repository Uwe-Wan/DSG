using System;

namespace DSG.Common.Exceptions
{
    public class NotEnoughCardsAvailableException : Exception
    {
        public NotEnoughCardsAvailableException()
        {
        }

        public NotEnoughCardsAvailableException(string message)
            : base(message)
        {
        }

        public NotEnoughCardsAvailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
