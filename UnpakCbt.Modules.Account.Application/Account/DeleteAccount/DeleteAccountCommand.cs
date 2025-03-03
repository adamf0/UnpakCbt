using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Account.Application.Account.DeleteAccount
{
    public sealed record DeleteAccountCommand(
        Guid uuid
    ) : ICommand;
}
