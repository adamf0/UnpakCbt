using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Account.Application.Account.GetAccount
{
    public sealed record GetAccountDefaultQuery(Guid AccountUuid) : IQuery<AccountDefaultResponse>;
}
