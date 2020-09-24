using System.Collections.Generic;

namespace ApiWrapper.Abstractions
{
    public interface IPostRequest<T>
    {
        IPostRequest<T> SetData(T element);
        IPostRequest<T> SetData(IEnumerable<T> element);
        IResponse<T> Post();
    }
}