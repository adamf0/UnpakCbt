using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Account.Application.Account.GetAccount
{
    public sealed record GetAccountQuery(Guid AccountUuid) : IQuery<AccountResponse>;
}
