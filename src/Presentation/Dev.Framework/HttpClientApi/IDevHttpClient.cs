using System.Threading.Tasks;
using System.Collections.Generic;

namespace Dev.Framework.HttpClientApi
{
    public interface IDevHttpClient
    {
        Task PingAsync();
        Task<string> GetResource(string resourceKey);
        Task<List<T>> GetAllAsync<T>(string url);
        Task<T> GetAsync<T>(string url);
    }
}
