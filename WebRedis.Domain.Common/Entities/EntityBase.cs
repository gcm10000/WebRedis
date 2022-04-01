using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Interfaces;

namespace WebRedis.Domain.Common.Entities
{
    public abstract class EntityBase : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public EntityBase()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
                CreatedAt = DateTime.Now;
            }
            UpdatedAt = DateTime.Now;
        }
    }
}
