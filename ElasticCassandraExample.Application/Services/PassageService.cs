using ElasticCassandraExample.Application.Interfaces;
using ElasticCassandraExample.Core.Domain.Passage;
using ElasticCassandraExample.Core.Interfaces;
using EllasticExample.DTO.Models;
using Nest;

namespace ElasticCassandraExample.Application.Services
{
    public class PassageService : IPassageService
    {
        private readonly IPassageRepository _passageRepository;

        public PassageService(IPassageRepository passageRepository)
        {
            _passageRepository = passageRepository;
        }

        public async Task<bool> AddOrUpdateAsync(Passage entity)
        {

            await _passageRepository.AddAsync(entity);

            return true;

        }

        public async Task<bool> AddOrUpdateAsync(List<Passage> passages)
        {
            await _passageRepository.AddAsync(passages);
            return true;
        }

        public async Task<List<Passage>> GetAllAsync()
        {
            var passages = await _passageRepository.GetAllAsync();

            return passages;
        }

        public async Task<List<Passage>> GetPassagesByLicensePlateAsync(string value)
        {
            return await _passageRepository.GetPassagesByLicensePlateAsync(value);
        }

        public async Task<FilterResponseDTO<Passage>> GetByDateMonthEquipmentCode(DateTime startDate, DateTime endDate, int idMonth, string codeEquipment)
        {
            var result = await _passageRepository.GetByDateMonthEquipmentCode(startDate, endDate, idMonth, codeEquipment);

            return new FilterResponseDTO<Passage>
            {
                Items = result.Documents.ToList(),
                PaginationScroolId = result.ScrollId
            };
        }

        public async Task<bool> InsertDataAsync(List<Passage> passages)
        {
            return await _passageRepository.InsertData(passages);
        }

        public async Task<bool> DeleteAllDocumentsByIndexNameAsync(string indexName)
        {
            return await _passageRepository.DeleteAllDocumentsAsync(indexName);
        }

        public async Task<FilterResponseDTO<Passage>> GetSearchByScrollId(string scrollTime, string paginationScrollId)
        {
            var result = await _passageRepository.GetSearchByScrollId(scrollTime, paginationScrollId);

            return new FilterResponseDTO<Passage>
            {
                Items = result.Documents.ToList(),
                PaginationScroolId = result.ScrollId
            };
        }
    }
}
