using AppBootstrap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBootstrap.Data
{
    
    public interface IBaseDAO<T> where T : BaseEntity
    {
        Task<List<T>> RetrieveAllAsync();
        Task<List<T>> RetrieveByIdsAsync(IEnumerable<long> Ids);
        Task<List<T>> RetrieveAllAsync(string UserCode);
        Task<T> SaveAsync(T item);
        Task<IEnumerable<T>> SaveAsync(IEnumerable<T> item);
        Task<T> UpdateAsync(T item);
        Task<T> RemoveAsync(T item);
        Task<bool> Exists(long Id);
        Task<int> RemoveAsync(IEnumerable<long> ids);
        Task<T> RetrieveByIdAsync(long Id);
        List<T> FindWithPaging(Dictionary<string, object> propertyValuePairs, int pageIndex, int maxSize, out int total);
        Task<(List<T>, int)> FindWithPagingAsync(Dictionary<string, object> propertyValuePairs, int pageNum, int maxSize);

        Task<(int, List<T>)> FilterPagedAsync(Dictionary<string, (string, string)> keyValuePairs, int pageNum, int maxSize);
    }
}
