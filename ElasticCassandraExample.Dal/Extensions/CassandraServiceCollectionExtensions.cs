using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticCassandraExample.Dal.Extensions
{
    public static class CassandraServiceCollectionExtensions
    {

        public static void AddCassandra(this IServiceCollection services, IConfiguration configuration)
        {

            var contactPoint = configuration["CassandraSettings:CassandraContactPoint"];
            var port = int.Parse(configuration["CassandraSettings:Port"]);
            var keyspace = configuration["CassandraSettings:Keyspace"];
            var username = configuration["CassandraSettings:Username"];
            var password = configuration["CassandraSettings:Password"];
            // Configuração do Cluster
            var cluster = Cluster.Builder()
                .AddContactPoint(contactPoint)
                .WithPort(port);

            // Configuração de autenticação (se necessário)
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                cluster.WithCredentials(username, password);

            // Conectar ao Keyspace especificado
            var session = cluster.Build().Connect(keyspace);

            // Registrar ISession no contêiner de DI
            services.AddSingleton(session);
        }

    }
}
