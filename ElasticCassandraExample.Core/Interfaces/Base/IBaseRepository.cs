using ElasticCassandraExample.Core.Domain.Base;
using ElasticCassandraExample.Core.Domain.Passage;
using Nest;

namespace ElasticCassandraExample.Domain.Interfaces.Base
{
    public interface IBaseRepository<T> where T : Passage
    {
        Task AddAsync(T entity);
        Task AddAsync(List<T> entity);
        Task<T> GetAsync(string value);

        Task<List<T>> GetAllAsync();
        Task<T> DeleteAsync();

        Task<ISearchResponse<T>> GetSearchByScrollId(string scrollTime,string PaginationScroolId);

    }
}
