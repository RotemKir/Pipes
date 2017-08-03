using System;
using System.Linq;
using System.Reflection;

namespace Pipes.Core
{
    internal class SegmentMethodInvoker : ISegmentMethodInvoker
    {
        public R InvokeMethod<R>(ISegment segment, string methodName, IPipelineContext context, R defaultValue)
        {
            if (segment == null) throw new ArgumentNullException("segment");
            if (methodName == null) throw new ArgumentNullException("methodName");
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException("Method name cannot be empty", "methodName");
            if (context == null) throw new ArgumentNullException("context");

            var methodToinvoke = GetMethod(segment.GetType(), methodName);

            if (methodToinvoke == null) return defaultValue;

            var parameters = GetParameters(methodToinvoke, context);
            object result = methodToinvoke.Invoke(segment, parameters);

            return result is R ? (R)result : defaultValue;
        }

        private MethodInfo GetMethod(Type segmentType, string methodName)
        {
            return segmentType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public);
        }

        private object[] GetParameters(MethodInfo method, IPipelineContext context)
        {
            var parametersInfos = method.GetParameters();
            var parameters = parametersInfos.Select(p => GetParameterValue(p, context)).ToArray();

            return parameters;
        }

        private object GetParameterValue(ParameterInfo parameterInfo, IPipelineContext context)
        {
            return GetParameterValueFromContext(parameterInfo, context) ??
                   GetParameterDefaultValue(parameterInfo);
        }

        private object GetParameterValueFromContext(ParameterInfo parameterInfo, IPipelineContext context)
        {
            return context.GetValue(parameterInfo.Name);
        }

        private object GetParameterDefaultValue(ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType.IsValueType)
            {
                return Activator.CreateInstance(parameterInfo.ParameterType);
            }

            return null;
        }
    }
}