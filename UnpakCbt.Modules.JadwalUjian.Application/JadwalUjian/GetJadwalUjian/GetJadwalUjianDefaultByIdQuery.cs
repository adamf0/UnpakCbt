﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian
{
    public sealed record GetJadwalUjianDefaultByIdQuery(int? id) : IQuery<JadwalUjianDefaultResponse>;
}
