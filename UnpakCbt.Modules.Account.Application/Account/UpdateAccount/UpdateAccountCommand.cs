using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Account.Application.Account.UpdateAccount
{
    public sealed record UpdateAccountCommand(
        Guid Uuid,
        string Username,
        string? Password,
        string Level
    ) : ICommand;
}
