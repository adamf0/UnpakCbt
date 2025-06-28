using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.LogEvent
{
    public sealed class LogCreatedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; init; } = eventId;
    }
}
