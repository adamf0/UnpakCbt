using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban
{
    public sealed class TemplateJawabanCreatedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; init; } = eventId;
    }
}
