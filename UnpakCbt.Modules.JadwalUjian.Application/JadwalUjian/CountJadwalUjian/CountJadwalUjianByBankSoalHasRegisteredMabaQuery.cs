using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.CountJadwalUjian
{
    public sealed record CountJadwalUjianByBankSoalHasRegisteredMabaQuery(Guid BankSoalUuid) : IQuery<int>;
}
