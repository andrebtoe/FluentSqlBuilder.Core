using System;

namespace SqlBuilderFluent.Exceptions
{
    public class SqlBuilderException : Exception
    {
        public SqlBuilderException(string message) : base(message) { }
    }
}