using ApiWrapper.Abstractions;

namespace ApiWrapper
{
    public class Api<T> : IApi<T>
    {
        public IHttpClient Client { get; set; }
        public Api(IHttpClient client)
        {
            Client = client;
            (GetRequest as GetRequest<T>).SetHttpClient(client);
            (PostRequest as PostRequest<T>).SetHttpClient(client);
        }

        public IGetRequest<T> GetRequest
        {
            get
            {
                var request = new GetRequest<T>();
                request.SetHttpClient(Client);
                return request;
            }
        }

        public IPostRequest<T> PostRequest
        {
            get
            {
                var request = new PostRequest<T>();
                request.SetHttpClient(Client);
                return request;
            }
        }

    }
}