using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateCbt
{
    public sealed record UpdateCbtCommand(
        Guid Uuid,
        string NoReg,
        Guid IdJadwalUjian,
        Guid IdJawabanBenar
    ) : ICommand;
}
