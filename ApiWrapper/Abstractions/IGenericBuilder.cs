using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApiWrapper.Abstractions
{
    public interface IGenericBuilder<T>
    {
        IDictionary<string, object> BuildDictionary();
        T BuildObject();
        IGenericBuilder<T> Set<Key>(Expression<Func<T, Key>> selector, object value);
    }
}