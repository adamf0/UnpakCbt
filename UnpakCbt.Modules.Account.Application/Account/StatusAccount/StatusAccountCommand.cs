using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Account.Application.Account.StatusAccount
{
    public sealed record StatusAccountCommand(
        Guid Uuid,
        string Status
    ) : ICommand;
}
