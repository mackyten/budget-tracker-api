using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.DOMAIN.Entities
{
    public abstract class BaseEntity<T> where T : struct
    {
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedById { get; set; }
        public string? LastUpdatedBy { get; set; }
        public bool IsGlobal { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeprecated { get; set; }
        public int Version { get; set; }
    }
}