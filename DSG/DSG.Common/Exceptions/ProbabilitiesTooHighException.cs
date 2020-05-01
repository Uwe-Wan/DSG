using System;

namespace DSG.Common.Exceptions
{
    public class ProbabilitiesTooHighException : Exception
    {
        public ProbabilitiesTooHighException()
        {
        }

        public ProbabilitiesTooHighException(string message)
            : base(message)
        {
        }

        public ProbabilitiesTooHighException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
