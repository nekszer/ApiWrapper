using ApiWrapper.Abstractions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ApiWrapper
{
    public class PostRequest<T> : Request<T>, IPostRequest<T>
    {
        private string EndPoint { get; set; } = $"v1/{typeof(T).Name.ToLower()}";
        private T Element { get; set; }

        private IEnumerable<T> Elements { get; set; }

        public override async Task<IEnumerable<K>> Execute<K>()
        {
            var props = new List<Parameter>();
            if (Headers != null)
                foreach (var item in Headers)
                    props.Add(new Parameter(item.Key, item.Value, ParameterType.HttpHeader));

            if (Client == null) return default(IEnumerable<K>);

            if (Element != null)
            {
                var propertiesinfo = Element.GetType().GetProperties();
                foreach (PropertyInfo propertyinfo in propertiesinfo)
                {
                    var apiparameter = (ApiParameter)propertyinfo.GetCustomAttributes().FirstOrDefault(p => p is ApiParameter);
                    if (apiparameter != null && apiparameter.Ignore) continue;
                    var key = propertyinfo.Name;
                    var value = propertyinfo.GetValue(Element, null);
                    if (!IsNullOrDefault(value))
                        props.Add(new Parameter(key, value != null ? value.ToString() : "", ParameterType.GetOrPost));
                }
            }

            var client = Client.SetParams(props)
                .SetMethod(Method.POST)
                .SetLogger(new ConsoleLogger());

            if (Elements != null)
                return await client.SetBody(new { data = Elements }).ExecuteAsync<IEnumerable<K>>(EndPoint);
            else
                return new List<K>(1) { await client.ExecuteAsync<K>(EndPoint) };
        }

        private bool IsNullOrDefault<X>(X argument)
        {
            if (argument == null) return true;
            if (Equals(argument, default(X))) return true;
            Type methodType = typeof(X);
            if (Nullable.GetUnderlyingType(methodType) != null) return false;
            Type argumentType = argument.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                object obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }
            return false;
        }

        public IPostRequest<T> SetData(T element)
        {
            if (element == null) throw new NullReferenceException("el parametro element es null");
            Element = element;
            return this;
        }

        public IPostRequest<T> SetData(IEnumerable<T> elements)
        {
            if (elements == null) throw new NullReferenceException("el parametro elements es null");
            Elements = elements;
            return this;
        }

        public IResponse<T> Post()
        {
            return new Response<T>(Execute<T>());
        }

    }
}
