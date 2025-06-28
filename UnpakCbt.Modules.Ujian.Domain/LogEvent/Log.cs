using System;
using System.ComponentModel.DataAnnotations.Schema;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.LogEvent
{
    public sealed partial class Log : Entity
    {
        private Log() { }

        public int Id { get; private set; }

        [Column(TypeName = "VARCHAR(36)")]
        public Guid Uuid { get; private set; }

        [Column("noreg")]
        public string NoReg { get; private set; }

        [Column("event")]
        public string Events { get; private set; } // Simpan JSON, teks panjang, dsb.

        [Column("created_at")]
        public DateTime CreatedAt { get; private set; } // Tipe waktu, bukan string

        public static Result<Log> Create(string noReg, string events)
        {
            var log = new Log
            {
                Uuid = Guid.NewGuid(),
                NoReg = noReg,
                Events = events,
                CreatedAt = DateTime.UtcNow
            };

            log.Raise(new LogCreatedDomainEvent(log.Uuid));

            return log;
        }
    }
}
