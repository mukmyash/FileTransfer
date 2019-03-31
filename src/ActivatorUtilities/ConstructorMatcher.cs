using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;

namespace ActivatorUtilities
{
    internal class ConstructorMatcher
    {
        private readonly ConstructorInfo _constructor;
        private readonly ParameterInfo[] _parameters;
        private readonly object[] _parameterValues;
        private readonly bool[] _parameterValuesSet;

        public ConstructorMatcher(ConstructorInfo constructor)
        {
            _constructor = constructor;
            _parameters = _constructor.GetParameters();
            _parameterValuesSet = new bool[_parameters.Length];
            _parameterValues = new object[_parameters.Length];
        }

        public int Match(object[] givenParameters)
        {
            var applyIndexStart = 0;
            var applyExactLength = 0;
            for (var givenIndex = 0; givenIndex != givenParameters.Length; givenIndex++)
            {
                var givenType = givenParameters[givenIndex]?.GetType().GetTypeInfo();
                var givenMatched = false;

                for (var applyIndex = applyIndexStart; givenMatched == false && applyIndex != _parameters.Length; ++applyIndex)
                {
                    if (_parameterValuesSet[applyIndex] == false &&
                        _parameters[applyIndex].ParameterType.GetTypeInfo().IsAssignableFrom(givenType))
                    {
                        givenMatched = true;
                        _parameterValuesSet[applyIndex] = true;
                        _parameterValues[applyIndex] = givenParameters[givenIndex];
                        if (applyIndexStart == applyIndex)
                        {
                            applyIndexStart++;
                            if (applyIndex == givenIndex)
                            {
                                applyExactLength = applyIndex;
                            }
                        }
                    }
                }

                if (givenMatched == false)
                {
                    return -1;
                }
            }
            return applyExactLength;
        }

        public object CreateInstance(IServiceProvider provider)
        {
            for (var index = 0; index != _parameters.Length; index++)
            {
                if (_parameterValuesSet[index] == false)
                {
                    var value = provider.GetService(_parameters[index].ParameterType);
                    if (value == null)
                    {
                        if (!ParameterDefaultValue.TryGetDefaultValue(_parameters[index], out var defaultValue))
                        {
                            throw new InvalidOperationException($"Unable to resolve service for type '{_parameters[index].ParameterType}' while attempting to activate '{_constructor.DeclaringType}'.");
                        }
                        else
                        {
                            _parameterValues[index] = defaultValue;
                        }
                    }
                    else
                    {
                        _parameterValues[index] = value;
                    }
                }
            }

            try
            {
                return _constructor.Invoke(_parameterValues);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                // The above line will always throw, but the compiler requires we throw explicitly.
                throw;
            }
        }
    }
}