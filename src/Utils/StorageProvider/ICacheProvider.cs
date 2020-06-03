using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace library_volunteering_enquiry_service.Utils.StorageProvider
{
    public interface ICacheProvider
    {
        Task<string> GetStringAsync(string key);

        Task SetStringAsync(string key, string value);

        Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options);
    }
}
