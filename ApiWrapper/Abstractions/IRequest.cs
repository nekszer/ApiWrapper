using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWrapper.Abstractions
{
    public interface IRequest<T>
    {
        IRequest<T> SetHeaders(IDictionary<string, string> headers);
        IRequest<T> SetHttpClient(IHttpClient httpclient);
    }
}
