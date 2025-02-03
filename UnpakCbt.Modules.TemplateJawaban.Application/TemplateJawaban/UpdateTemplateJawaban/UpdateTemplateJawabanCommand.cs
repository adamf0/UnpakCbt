﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.UpdateTemplateJawaban
{
    public sealed record UpdateTemplateJawabanCommand(
        Guid Uuid,
        Guid IdTemplateSoal,
        string? JawabanText = null,
        string? JawabanImg = null
    ) : ICommand;
}
