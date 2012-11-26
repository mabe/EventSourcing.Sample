using System;

namespace EventSourcing.Sample.Core.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class NoEventHandlerAttribute : Attribute
    {
    }
}