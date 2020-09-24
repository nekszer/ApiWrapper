using ApiWrapper.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ApiWrapper
{
    public class GenericBuilder<T> : IGenericBuilder<T>
    {
        private T Element { get; set; }
        private Dictionary<string, object> Data { get; set; }

        public GenericBuilder()
        {
            Element = Activator.CreateInstance<T>();
            Data = new Dictionary<string, object>();
        }

        public IGenericBuilder<T> Set<Key>(Expression<Func<T, Key>> selector, object value)
        {
            var prop = (PropertyInfo)((MemberExpression)selector.Body).Member;
            var apiparameter = (ApiParameter) prop.GetCustomAttributes().FirstOrDefault(p => p is ApiParameter);
            if (apiparameter != null && apiparameter.Ignore) return this;
            prop.SetValue(Element, value, null);
            Data.Add(prop.Name.ToLower(), value);
            return this;
        }

        public T BuildObject()
        {
            return Element;
        }

        public IDictionary<string, object> BuildDictionary()
        {
            return Data;
        }
    }
}
