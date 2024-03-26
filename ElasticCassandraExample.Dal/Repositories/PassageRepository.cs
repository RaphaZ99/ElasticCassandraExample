using Cassandra;
using ElasticCassandraExample.Core.Domain.Passage;
using ElasticCassandraExample.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ElasticCassandraExample.Dal.Repositories
{
    public class PassageRepository : IPassageRepository
    {
        private readonly IElasticClient _elasticClient;
        private readonly ISession _session;

        public PassageRepository(IElasticClient elasticClient, ISession session)
        {
            _elasticClient = elasticClient;
            _session = session;
        }

        public Task AddAsync(Passage entity)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(List<Passage> entities)
        {
            try
            {
                //var query = "INSERT INTO passage (id_month, license_plate, date_time_passage, code_equipment, code_lane, axles, lane_direction, size_vehicle, speed_measured, url_image, vehicle_classification, brand_vehicle, color_vehicle, model_vehicle) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                //var statement = _session.Prepare(query);

                //foreach (var entity in entities)
                //{
                //    var boundStatement = statement.Bind(
                //        entity.IdMonth,
                //        entity.LicensePlate,
                //        entity.DateTimePassage,
                //        entity.CodeEquipment,
                //        entity.CodeLane,
                //        entity.Axles,
                //        entity.LaneDirection,
                //        entity.SizeVehicle,
                //        entity.SpeedMeasured,
                //        entity.UrlImage,
                //        entity.VehicleClassification,
                //        entity.BrandVehicle,
                //        entity.ColorVehicle,
                //        entity.ModelVehicle
                //    );
                //    await _session.ExecuteAsync(boundStatement);

                //    var response = await _elasticClient.IndexDocumentAsync(entity);

                //    if (!response.IsValid)
                //    {
                //        Console.WriteLine($"Não foi possível inserir o dado no ellastic server {entity.LicensePlate} - {response.DebugInformation}");
                //    }
                //}

                var descriptor = new BulkDescriptor();

                descriptor.CreateMany<Passage>(entities);

                var insert = await _elasticClient.BulkAsync(descriptor);

            }
            catch (Exception ex)
            {
                throw new Exception(message: $"Não foi possível realizar a persistência dos dados {ex.Message} - {ex.StackTrace}");
            }
        }

        public Task<Passage> GetAsync(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Passage>> GetPassagesByLicensePlateAsync(string value)
        {
            var result = _elasticClient.Search<Passage>(x => x
                .Index("passage")
                .Query(q => q
                    .Bool(bo => bo
                        .Must(mus => mus.Match(m => m.Field(f => f.LicensePlate).Query(value)))))
                .Scroll("1m"));


            List<Passage> results = new List<Passage>();

            if (result.Documents.Any())
                results.AddRange(result.Documents);

            string scrollid = result.ScrollId;
            bool isScrollSetHasData = true;

            while (isScrollSetHasData)
            {

                ISearchResponse<Passage> loopingResponse = _elasticClient.Scroll<Passage>("1m", scrollid);

                if (loopingResponse.IsValid)
                {
                    results.AddRange(loopingResponse.Documents);
                    scrollid = loopingResponse.ScrollId;
                }

                isScrollSetHasData = loopingResponse.Documents.Any();
            }

            _elasticClient.ClearScroll(new ClearScrollRequest(scrollid));


            return results.ToList();
        }

        public async Task<List<Passage>> GetAllAsync()
        {
            var result = _elasticClient.Search<Passage>(x => x
                .Index("passage")
                .From(0)
                .Size(100)
                .Scroll("1m")
                .MatchAll());

            List<Passage> results = new List<Passage>();

            if (result.Documents.Any())
                results.AddRange(result.Documents);

            string scrollid = result.ScrollId;
            bool isScrollSetHasData = true;

            while (isScrollSetHasData)
            {

                ISearchResponse<Passage> loopingResponse = _elasticClient.Scroll<Passage>("1m", scrollid);

                if (loopingResponse.IsValid)
                {
                    results.AddRange(loopingResponse.Documents);
                    scrollid = loopingResponse.ScrollId;
                }

                isScrollSetHasData = loopingResponse.Documents.Any();
            }

            _elasticClient.ClearScroll(new ClearScrollRequest(scrollid));


            return results.ToList();
        }

        public Task<Passage> DeleteAsync()
        {
            throw new NotImplementedException();
        }


        public async Task<bool> InsertData(List<Passage> passages)
        {

            var listChunck = passages.Chunk(5000).ToList();


            foreach (var item in listChunck)
            {


                var waitHandle = new CountdownEvent(1);

                var bulkAll = _elasticClient.BulkAll(item, b => b
                    .Index("passage") /* index */
                    .BackOffRetries(2)
                    .BackOffTime("1m")
                    .RefreshOnCompleted(true)
                    .MaxDegreeOfParallelism(4)
                    .Size(5000)
                );

                bulkAll.Subscribe(new BulkAllObserver(
                    onNext: (b) => { Console.Write("."); },
                    onError: (e) => { throw e; },
                    onCompleted: () => waitHandle.Signal()
                ));

                waitHandle.Wait();


                //var descriptor = new BulkDescriptor();

                //descriptor.UpdateMany<Passage>(item, (b, u) => b
                //    .Index("passage")
                //    .Doc(u)
                //    .DocAsUpsert());

                //var insert = await _elasticClient.BulkAsync(descriptor);

                //if (!insert.IsValid)
                //    throw new Exception(insert.OriginalException.ToString());
            }

            return true;
        }

        public async Task<bool> DeleteAllDocumentsAsync(string indexName)
        {
            var response = await _elasticClient.DeleteByQueryAsync<Passage>(d => d
                    .Index(indexName)
                    .Query(q => q.MatchAll())
            );
            if (!response.IsValid)
            {

                throw new Exception("Falha ao deletar documentos: " + response.DebugInformation);
            }
            return true;
        }

        public async Task<ISearchResponse<Passage>> GetByDateMonthEquipmentCode(DateTime startDate, DateTime endDate, int idMonth, string codeEquipment)
        {
            var result = await _elasticClient.SearchAsync<Passage>(s => s
                .Index("passage")
                .Query(q => q
                    .Bool(b => b
                        .Must(mu => mu
                                .DateRange(r => r
                                    .Field(f => f.DateTimePassage)
                                    .GreaterThanOrEquals(startDate.ToString("yyyy-MM-ddTHH:mm:ss"))
                                    .LessThanOrEquals(endDate.ToString("yyyy-MM-ddTHH:mm:ss"))
                                ),
                            //mu => mu
                            //    .Match(m => m
                            //        .Field(f => f.CodeEquipment)
                            //        .Query(codeEquipment)
                            //    ),
                            mu => mu
                                .Match(m => m
                                    .Field(f => f.IdMonth)
                                    .Query(idMonth.ToString())
                                )
                        )
                    )
                )
                .Size(100)
                .Scroll("1m"));

            return result;
             
        }

        public async Task<ISearchResponse<Passage>> GetSearchByScroolId(string scrollTime, string PaginationScroolId)
        {
            var response = await _elasticClient.ScrollAsync<Passage>(scrollTime, PaginationScroolId);

            if (!response.IsValid)
            {
                throw new Exception("Falha ao recuperar resultados do scroll: " + response.DebugInformation);

            }

            return response;
        }
    }
}
