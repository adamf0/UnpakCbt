using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Account.Domain.Account
{
    public sealed partial class Account : Entity
    {
        private Account()
        {
        }

        public int? Id { get; private set; } = null;

        [Column(TypeName = "VARCHAR(36)")]
        public Guid Uuid { get; private set; }

        public string Username { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public string Level { get; private set; } = null;
        public string Status { get; private set; } = null;

        public static AccountBuilder Update(Account prev) => new AccountBuilder(prev);

        public static Result<Account> Create(
        string Username,
        string Password,
        string Level,
        string Status
        )
        {
            string pattern1 = @"[!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?/\\]";
            //string pattern2 = @"^[0-9]+$";

            if (string.IsNullOrWhiteSpace(Username)) {
                return Result.Failure<Account>(AccountErrors.EmptyUsername());
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                return Result.Failure<Account>(AccountErrors.EmptyPassword());
            }
            if (Password.Length<8)
            {
                return Result.Failure<Account>(AccountErrors.MinPassword());
            }
            if (!Regex.IsMatch(Password, pattern1))
            {
                return Result.Failure<Account>(AccountErrors.SpecialCharcterPassword());
            }
            /*if (!Regex.IsMatch(Password, pattern2))
            {
                return Result.Failure<Account>(AccountErrors.NumberPassword());
            }*/
            if (string.IsNullOrWhiteSpace(Level))
            {
                return Result.Failure<Account>(AccountErrors.EmptyLevel());
            }
            if (Level!="admin")
            {
                return Result.Failure<Account>(AccountErrors.InvalidLevel(Level));
            }

            var asset = new Account
            {
                Uuid = Guid.NewGuid(),
                Username = Username,
                Password = Password,
                Level = Level,
                Status = Status
            };

            asset.Raise(new AccountCreatedDomainEvent(asset.Uuid));

            return asset;
        }
    }
}
