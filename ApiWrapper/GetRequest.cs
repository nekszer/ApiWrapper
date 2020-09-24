using ApiWrapper.Abstractions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWrapper
{
    public class GetRequest<T> : Request<T>, IGetRequest<T>
    {

        private string EndPoint { get; set; } = $"v1/{typeof(T).Name.ToLower()}";
        private int Limit { get; set; }
        private int Offset { get; set; }
        private IDictionary<string, object> Where { get; set; }
        private object Id { get; set; }

        public IGetRequest<T> SetLimit(int limit)
        {
            Limit = limit;
            return this;
        }

        public IGetRequest<T> SetOffset(int offset)
        {
            Offset = offset;
            return this;
        }

        public IGetRequest<T> SetWhere(IGenericBuilder<T> ApiWrapper)
        {
            if (ApiWrapper == null) throw new NullReferenceException("el parametro ApiWrapper es null");
            Where = ApiWrapper.BuildDictionary();
            return this;
        }

        public IGetRequest<T> SetId(object id)
        {
            if (id == null) throw new NullReferenceException("el parametro id es null");
            Id = id;
            return this;
        }

        public IResponse<T> Get()
        {
            return new Response<T>(Execute<T>());
        }

        public override async Task<IEnumerable<K>> Execute<K>()
        {
            var props = new List<Parameter>();
            if (Where != null)
                foreach (var item in Where)
                    props.Add(new Parameter(item.Key, item.Value, ParameterType.GetOrPost));

            if (Headers != null)
                foreach (var item in Headers)
                    props.Add(new Parameter(item.Key, item.Value, ParameterType.HttpHeader));

            if (Limit > 0)
                props.Add(new Parameter("limit", Limit, ParameterType.GetOrPost));

            if (Offset > 0)
                props.Add(new Parameter("offset", Offset, ParameterType.GetOrPost));

            if (Id != null)
                EndPoint += $"/{Id}";

            if (Client == null) return default(IEnumerable<K>);
            if (Id != null)
            {
                var k = await Client.SetParams(props)
                    .SetMethod(Method.GET)
                    .SetLogger(new ConsoleLogger())
                    .ExecuteAsync<K>(EndPoint);
                return new List<K>(1) { k };
            }
            else
            {
                return await Client.SetParams(props)
                    .SetMethod(Method.GET)
                    .SetLogger(new ConsoleLogger())
                    .ExecuteAsync<IEnumerable<K>>(EndPoint);
            }
        }
    }
}
