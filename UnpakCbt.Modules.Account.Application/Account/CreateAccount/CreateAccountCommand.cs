using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Account.Application.Account.CreateAccount
{
    public sealed record CreateAccountCommand(
        string Username,
        string Password,
        string Level
    ) : ICommand<Guid>;
}
