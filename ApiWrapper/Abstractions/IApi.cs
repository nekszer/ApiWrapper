namespace ApiWrapper.Abstractions
{
    public interface IApi<T>
    {
        IGetRequest<T> GetRequest { get; }
        IPostRequest<T> PostRequest { get; }
    }
}