using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Common.Domain
{
    public abstract class DomainEvent : IDomainEvent
    {
        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOnUtc = DateTime.UtcNow;
        }

        protected DomainEvent(Guid id, DateTime occurredOnUtc)
        {
            Id = id;
            OccurredOnUtc = occurredOnUtc;
        }

        public Guid Id { get; init; }

        public DateTime OccurredOnUtc { get; init; }
    }
}
