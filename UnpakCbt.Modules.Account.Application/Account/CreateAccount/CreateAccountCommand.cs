using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Account.Application.Account.CreateAccount
{
    public sealed record authenticationCommand(
        string Username,
        string Password,
        string Level
    ) : ICommand<Guid>;
}
