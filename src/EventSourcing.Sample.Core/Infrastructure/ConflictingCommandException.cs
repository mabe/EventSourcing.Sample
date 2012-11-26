using System;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public class ConflictingCommandException : Exception
    {
        public ConflictingCommandException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}