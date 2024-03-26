using ElasticCassandraExample.Core.Domain.Base;
using ElasticCassandraExample.Core.Domain.Passage;
using EllasticExample.DTO.Models;

namespace ElasticCassandraExample.Application.Base
{
    public interface IBaseService<T>
    {
        Task<bool> AddOrUpdateAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<bool> InsertDataAsync(List<T> passages);

        Task<bool> DeleteAllDocumentsByIndexNameAsync(string indexName);

        Task<FilterResponseDTO<T>> GetSearchByScrollId(string scrollTime, string PaginationScroolId);
    }
}
