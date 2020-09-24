using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWrapper.Abstractions
{
    public interface IHttpClient
    {
        Task<T> ExecuteAsync<T>(string endpoint);
        IHttpClient OverwriteBaseUrlForRequest(string baseurl);
        IHttpClient SetLogger(ILogger logger);
        IHttpClient SetMethod(Method method);
        IHttpClient SetParams(IEnumerable<Parameter> parameters);
        IHttpClient SetBody(object body);
        IHttpClient SetFiles(IEnumerable<IFile> files);
        IHttpClient SetCookies(IDictionary<string,string> cookies);
        IHttpClient SetHeaders(IDictionary<string, string> headers);
        IDictionary<string,string> GetResponseCookies();
        IDictionary<string, string> GetResponseHeaders();
    }
}