﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateUjian
{
    public sealed record UpdateUjianCommand(
        Guid Uuid,
        string NoReg,
        Guid IdJadwalUjian,
        string Status
    ) : ICommand;
}
