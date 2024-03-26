using Cassandra;

namespace ElasticCassandraExample.Cassandra
{
    public class CassandraContext : IDisposable
    {

        private ISession? _session; 
        private readonly CassandraSettings Settings;
        private readonly Cluster Cluster;

        public ISession Session => _session ??= Cluster.Connect(Settings.KeySpace).Cluster;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
