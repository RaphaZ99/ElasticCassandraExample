using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticCassandraExample.Core.Domain.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
    }
}
