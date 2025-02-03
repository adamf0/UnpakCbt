﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban
{
    public sealed record TemplateJawabanResponse
    {
        public string Uuid { get; set; }
        public string IdTemplateSoal { get; set; }
        public string JawabanText { get; set; }
        public string JawabanImg { get; set; }
    }
}
