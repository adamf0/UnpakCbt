using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.Account.Application.Account.GetAccount;

namespace UnpakCbt.Modules.Account.Application.Account.GetAllAccount
{
    public sealed record GetAllAccountQuery() : IQuery<List<AccountResponse>>;
}
