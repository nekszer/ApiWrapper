using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiWrapper.Abstractions
{
    public interface IResponse<T>
    {
        Task<T> Row();
        Task<IEnumerable<T>> Result();
    }
}
