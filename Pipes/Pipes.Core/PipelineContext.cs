using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pipes.Core
{
    internal class PipelineContext : IPipelineContext
    {
        private Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public int MaxVersion { get; set; } = int.MaxValue;

        public object GetValue(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", "name");

            var key = GetKey(name);

            return Data.ContainsKey(key) ? Data[key] : null;
        }

        public void SetValue(object value)
        {
            if (value == null) return;

            var resultType = value.GetType();

            if (IsAnonymousType(resultType))
            {
                SetAnonymousTypeInContext(value, resultType);
            }
        }

        private string GetKey(string name)
        {
            return name.ToLower();
        }

        private void SetAnonymousTypeInContext(object result, Type resultType)
        {
            var properties = resultType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var key = GetKey(property.Name);

                Data[key] = property.GetValue(result);
            }
        }

        private bool IsAnonymousType(Type type)
        {
            return type.Name.Contains("AnonymousType") && Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute));
        }
    }
}