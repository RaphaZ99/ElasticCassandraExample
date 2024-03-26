using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticCassandraExample.Cassandra
{
    public class CassandraSettings
    {
        public string KeySpace { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string CassandraContactPoint { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Porta
        /// </summary>
        public int Port { get; set; }
    }
}
