using ApiWrapper.Abstractions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiWrapper
{
    public class HttpClient : IHttpClient
    {
        private string BaseUrl { get; set; }
        private string CustomUrl { get; set; }
        private IList<ILogger> Logger { get; set; }
        private string Url { get; set; }
        private Method Method { get; set; } = Method.GET;
        private IEnumerable<Parameter> Parameters { get; set; }
        private string EndPoint { get; set; }
        private object Body { get; set; }
        private IEnumerable<IFile> Files { get; set; }

        public HttpClient(string baseurl)
        {
            BaseUrl = baseurl;
        }

        public async Task<T> ExecuteAsync<T>(string endpoint)
        {
            Url = "";
            Url = BaseUrl;
            if (!string.IsNullOrEmpty(CustomUrl))
            {
                Url = CustomUrl;
                CustomUrl = string.Empty;
            }

            if (!Url.EndsWith("/")) Url += "/";

            EndPoint = endpoint;

            var client = new RestClient(Url);
            var request = new RestRequest(endpoint, Method);

            if (Parameters != null)
                foreach (var parameter in Parameters)
                    request.AddParameter(parameter);

            if(Body != null)
                request.AddParameter("json", Newtonsoft.Json.JsonConvert.SerializeObject(Body), "application/json", ParameterType.RequestBody);

            if(Files != null)
                foreach (var file in Files)
                    request.AddFile(file.ParameterName, file.Bytes, file.FileName);

            if (Cookies != null)
                foreach (var cookie in Cookies)
                    request.AddCookie(cookie.Key, cookie.Value);

            if (Headers != null)
                foreach (var header in Headers)
                    request.AddHeader(header.Key, header.Value);

            IRestResponse<T> response = null;

            try
            {
                response = await client.ExecuteAsync<T>(request);
            }
            catch { }

            #region Print Info
            var log = GetLog(response);
            if(Logger != null)
                foreach (var logger in Logger)
                {
                    logger.Write(log);
                }
            #endregion

            Clean();

            if (response == null) throw new ApiException(string.Empty, HttpStatusCode.ServiceUnavailable);
            if (response.StatusCode != HttpStatusCode.OK) throw new ApiException(response.Content, response.StatusCode);

            if(response.Cookies != null && response.Cookies.Count > 0)
            {
                ResponseCookies = new Dictionary<string, string>();
                foreach (var cookie in response.Cookies)
                {
                    if(!ResponseCookies.ContainsKey(cookie.Name))
                        ResponseCookies.Add(cookie.Name, cookie.Value);
                    else
                        ResponseCookies[cookie.Name] = cookie.Value;
                }
            }

            return response.Data;
        }

        private void Clean()
        {
            Method = Method.GET;
            Parameters = null;
            EndPoint = null;
            Body = null;
            Files = null;
        }

        private string GetLog<T>(IRestResponse<T> response)
        {
            string log = "";
            log += "========================================================\r\n";
            log += "                        API LOG                         \r\n";
            log += "========================================================\r\n";
            log += $"BaseUrl: {Url}\r\n";
            log += Method + ": " + EndPoint + "\r\n";

            var headers = new List<string>();
            if (Parameters != null)
                foreach (var parameter in Parameters.Where(p => p.Type == ParameterType.HttpHeader))
                {
                    headers.Add("\"" + parameter.Name + "\":\"" + parameter.Value + "\"");
                }

            var headerjson = "{ " + string.Join(",", headers) + " }\r\n";
            log += "Headers:" + headerjson + "\r\n";

            log += "Code:" + (response?.StatusCode ?? HttpStatusCode.InternalServerError).ToString() + "\r\n";

            var props = new List<string>();
            if(Parameters != null)
                foreach (var parameter in Parameters.Where(p => p.Type == ParameterType.GetOrPost))
                {
                    props.Add("\"" + parameter.Name + "\":\"" + parameter.Value + "\"");
                }

            var formjson = "{ " + string.Join(",", props) + " }\r\n";
            log += "Request:" + formjson + "\r\n";
            log += "Response: " + (response?.Content ?? "Empty") + "\r\n";
            log += "========================================================\r\n";
            log += "                      END API LOG                       \r\n";
            log += "========================================================\r\n";
            return log;
        }

        public IHttpClient OverwriteBaseUrlForRequest(string baseurl)
        {
            CustomUrl = baseurl;
            return this;
        }

        public IHttpClient SetLogger(ILogger logger)
        {
            if (Logger == null) Logger = new List<ILogger>();
            Logger.Add(logger);
            return this;
        }

        public IHttpClient SetMethod(Method method)
        {
            Method = method;
            return this;
        }

        public IHttpClient SetParams(IEnumerable<Parameter> parameters)
        {
            Parameters = parameters;
            return this;
        }

        public IHttpClient SetBody(object body)
        {
            Body = body;
            return this;
        }

        public IHttpClient SetFiles(IEnumerable<IFile> files)
        {
            Files = files;
            return this;
        }

        private IDictionary<string, string> Cookies { get; set; }
        public IHttpClient SetCookies(IDictionary<string, string> cookies)
        {
            Cookies = cookies;
            return this;
        }

        private IDictionary<string, string> Headers { get; set; }
        public IHttpClient SetHeaders(IDictionary<string, string> headers)
        {
            Headers = headers;
            return this;
        }

        private Dictionary<string,string> ResponseCookies { get; set; }
        public IDictionary<string, string> GetResponseCookies()
        {
            return ResponseCookies;
        }

        private Dictionary<string, string> ResponseHeaders { get; set; }
        public IDictionary<string, string> GetResponseHeaders()
        {
            return ResponseHeaders;
        }
    }

}
