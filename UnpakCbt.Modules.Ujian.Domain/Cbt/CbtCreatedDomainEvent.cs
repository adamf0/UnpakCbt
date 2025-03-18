using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Cbt
{
    public sealed class CbtCreatedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; init; } = eventId;
    }
}
