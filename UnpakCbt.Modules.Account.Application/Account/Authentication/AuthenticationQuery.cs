using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Account.Application.Account.Authentication
{
    public sealed record AuthenticationQuery(string username, string password) : IQuery<string>;
}
