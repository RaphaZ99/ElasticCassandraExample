using ElasticCassandraExample.Application.Interfaces;
using ElasticCassandraExample.Application.Services;
using ElasticCassandraExample.Core.Interfaces;
using ElasticCassandraExample.Dal.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticCassandraExample.Dal.Extensions
{
    public static class RepositoryExtensions
    {

        public static void AddRepositoryExtensions(this IServiceCollection services)
        {

            services.AddScoped<IPassageService,PassageService>();
            services.AddScoped<IPassageRepository,PassageRepository>();
        }
    }
}
