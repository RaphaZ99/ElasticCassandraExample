using ElasticCassandraExample.Core.Domain.Passage;
using ElasticCassandraExample.Domain.Interfaces.Base;
using Nest;
using System.Threading.Tasks;

namespace ElasticCassandraExample.Core.Interfaces
{
    public interface IPassageRepository : IBaseRepository<Domain.Passage.Passage>
    {

        Task<List<Passage>> GetPassagesByLicensePlateAsync(string value);

        Task<bool> InsertData(List<Passage> passages);

        Task<bool> DeleteAllDocumentsAsync(string indexName);

        Task<ISearchResponse<Passage>> GetByDateMonthEquipmentCode(DateTime startDate, DateTime endDate, int idMonth, string codeEquipment);
    }
}
