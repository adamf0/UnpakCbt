﻿using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian
{
    public sealed class JadwalUjianCreatedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; init; } = eventId;
    }
}
