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
    public sealed record BankSoalDefaultResponse
    {
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string Judul { get; set; } = default!;
        public string Rule { get; set; } = "{}";
    }
}
