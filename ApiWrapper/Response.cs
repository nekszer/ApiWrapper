using ApiWrapper.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWrapper
{
    public class Response<T> : IResponse<T>
    {

        private Task<IEnumerable<T>> Task;
        public Response(Task<IEnumerable<T>> task)
        {
            Task = task;
        }

        public async Task<T> Row()
        {
            var result = await Task.ConfigureAwait(true);
            if (result == null) return default(T);
            return result.FirstOrDefault();
        }

        public Task<IEnumerable<T>> Result()
        {
            return Task;
        }

    }
}