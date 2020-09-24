using ApiWrapper.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWrapper
{
    public abstract class Request<T> : IRequest<T>
    {
        public IHttpClient Client { get; set; }
        public IDictionary<string, string> Headers { get; set; }

        public IRequest<T> SetHeaders(IDictionary<string, string> headers)
        {
            if (headers == null) throw new NullReferenceException("el parametro where es null");
            Headers = headers;
            return this;
        }

        public IRequest<T> SetHttpClient(IHttpClient httpclient)
        {
            if (httpclient == null) throw new NullReferenceException("el parametro httpclient es null");
            Client = httpclient;
            return this;
        }

        public abstract Task<IEnumerable<K>> Execute<K>();
    }
}
