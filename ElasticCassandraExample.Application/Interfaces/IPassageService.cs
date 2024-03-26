using ElasticCassandraExample.Application.Base;
using ElasticCassandraExample.Core.Domain.Passage;
using EllasticExample.DTO.Models;
using Nest;

namespace ElasticCassandraExample.Application.Interfaces
{
    public interface IPassageService : IBaseService<Passage>
    {

        Task<List<Passage>> GetPassagesByLicensePlateAsync(string value);

        Task<FilterResponseDTO<Passage>> GetByDateMonthEquipmentCode(DateTime startDate, DateTime endDate, List<int> idMonths, string codeEquipment);
    }
}
