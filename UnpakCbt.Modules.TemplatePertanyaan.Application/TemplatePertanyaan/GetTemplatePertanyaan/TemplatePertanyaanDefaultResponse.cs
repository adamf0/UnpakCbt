using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan
{
    public sealed record TemplatePertanyaanDefaultResponse
    {
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string IdBankSoal { get; set; }
        public string Tipe { get; set; }
        public string Pertanyaan { get; set; }
        public string Gambar { get; set; }
        public string JawabanBenar { get; set; }
        public string Bobot { get; set; }
        public string State { get; set; }
    }
}
