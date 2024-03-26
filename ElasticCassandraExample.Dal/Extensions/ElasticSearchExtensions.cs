using ElasticCassandraExample.Core.Domain.Passage;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ElasticCassandraExample.ElasticSearch.Extension
{
    public static class ElasticSearchExtensions
    {

        public static void AddElasticSearch(this IServiceCollection services, string basictAuthUser,string basictAuthPassword, string defaultIndex, string uri)
        {

            var settings = new ConnectionSettings(new Uri(uri));
             
            if (!string.IsNullOrEmpty(defaultIndex))
                settings = settings.DefaultIndex(defaultIndex);
             
            if (!string.IsNullOrEmpty(basictAuthUser) && !string.IsNullOrEmpty(basictAuthPassword))
                settings = settings.BasicAuthentication(basictAuthUser, basictAuthPassword);

            settings.EnableApiVersioningHeader();
            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client,defaultIndex);

        }

        private static void CreateIndex(IElasticClient client, string indexName) {
            var createIndexResponse = client.Indices.Create(indexName, index => index.Map <Passage> (x => x.AutoMap()));
        }

        private static void AddDefaultMappings(ConnectionSettings settings) {
            settings.DefaultMappingFor <Passage> (m => m.Ignore(p => p.CodeEquipment));
        }
    }
}
