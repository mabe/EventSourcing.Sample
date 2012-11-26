﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public class ConventionalEventHandlerMappingStrategy : IEventHandlerMappingStrategy
    {
        private const String RegexPattern = "^(on|On|ON)+";

        public IEnumerable<IEventHandler> GetEventHandlers(object target)
        {
            var targetType = target.GetType();

            var methodsToMatch = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var matchedMethods = from method in methodsToMatch
                                 let parameters = method.GetParameters()
                                 let noEventHandlerAttributes =
                                     method.GetCustomAttributes(typeof(NoEventHandlerAttribute), true)
                                 where
                                    Regex.IsMatch(method.Name, RegexPattern, RegexOptions.CultureInvariant) &&
                                    parameters.Length == 1 &&
                                    noEventHandlerAttributes.Length == 0
                                 select
                                    new { MethodInfo = method, FirstParameter = method.GetParameters()[0] };

            var shouldHandleMatchedMethods = from method in methodsToMatch
                                             let parameters = method.GetParameters()
                                             where method.Name == "ShouldHandle" &&
                                                   parameters.Length == 1 &&
                                                   method.ReturnType == typeof(bool)
                                             select
                                                 new { MethodInfo = method, FirstParameter = method.GetParameters()[0] };

            return (from method in matchedMethods
                    select method.MethodInfo
                        into methodCopy
                        let firstParameterType = methodCopy.GetParameters().First().ParameterType
                        let invokeAction = (Action<object>)(e => methodCopy.Invoke(target, new[] { e }))
                        let shouldHandleInvokeAction = (Func<object, bool>)(e =>
                                                                                {
                                                                                    var shouldHandleMethod = shouldHandleMatchedMethods.FirstOrDefault(x => x.FirstParameter.ParameterType.IsInstanceOfType(e));

                                                                                    if (shouldHandleMethod == null) return true;

                                                                                    return (bool)shouldHandleMethod.MethodInfo.Invoke(target, new[] { e });
                                                                                })
                        select new TypeThresholdedActionBasedDomainEventHandler(invokeAction, shouldHandleInvokeAction, firstParameterType)).Cast<IEventHandler>().ToList();
        }
    }
}