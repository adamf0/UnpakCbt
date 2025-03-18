using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.TemplatePertanyaan
{
    public sealed class TemplatePertanyaanCreatedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; init; } = eventId;
    }
}
