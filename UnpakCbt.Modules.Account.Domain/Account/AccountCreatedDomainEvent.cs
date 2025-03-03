using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Account.Domain.Account
{
    public sealed class AccountCreatedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; init; } = eventId;
    }
}
