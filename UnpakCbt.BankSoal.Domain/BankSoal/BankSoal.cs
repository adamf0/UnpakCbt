using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.BankSoal.Domain.BankSoal
{
    public sealed partial class BankSoal : Entity
    {
        private BankSoal()
        {
        }

        public int? Id { get; private set; } = null;

        [Column(TypeName = "VARCHAR(36)")]
        public Guid Uuid { get; private set; }

        public string Judul { get; private set; } = null!;
        public string? Rule { get; private set; } = null;
        
        public static BankSoalBuilder Update(BankSoal prev) => new BankSoalBuilder(prev);

        public static Result<BankSoal> Create(
        string Judul,
        string? Rule
        )
        {
            if (string.IsNullOrWhiteSpace(Judul)) {
                return Result.Failure<BankSoal>(BankSoalErrors.EmptyTitle());
            }

            var asset = new BankSoal
            {
                Uuid = Guid.NewGuid(),
                Judul = Judul,
                Rule = Rule
            };

            asset.Raise(new BankSoalCreatedDomainEvent(asset.Uuid));

            return asset;
        }
    }
}
