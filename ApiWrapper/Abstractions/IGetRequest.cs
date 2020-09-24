namespace ApiWrapper.Abstractions
{
    public interface IGetRequest<T>
    {
        IResponse<T> Get();
        IGetRequest<T> SetId(object id);
        IGetRequest<T> SetLimit(int limit);
        IGetRequest<T> SetOffset(int offset);
        IGetRequest<T> SetWhere(IGenericBuilder<T> ApiWrapper);
    }
}