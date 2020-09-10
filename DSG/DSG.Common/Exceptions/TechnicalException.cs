﻿using System;

namespace DSG.Common.Exceptions
{
    public class TechnicalException : Exception
    {
        public TechnicalException()
        {
        }

        public TechnicalException(string message)
            : base(message)
        {
        }

        public TechnicalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
