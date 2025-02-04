using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    public sealed record UjianResponse
    {
        public string Uuid { get; set; }
        public string NoReg { get; set; } = default!;
        public int JadwalUjian { get; set; }
    }
}
