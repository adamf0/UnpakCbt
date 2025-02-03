using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal
{
    public sealed record BankSoalResponse
    {
        public string Uuid { get; set; }
        public string Judul { get; set; } = default!;
        [Column(TypeName = "TEXT")]
        public string Rule { get; set; } = "{}";
    }
}
