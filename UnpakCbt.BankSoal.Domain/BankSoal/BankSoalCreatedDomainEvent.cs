using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.BankSoal.Domain.BankSoal
{
    public sealed class BankSoalCreatedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; init; } = eventId;
    }
}
