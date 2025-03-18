using System.Globalization;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.JadwalUjian
{
    public sealed partial class JadwalUjian
    {
        public sealed class JadwalUjianBuilder
        {
            private readonly JadwalUjian _akurasiPenelitian;
            private Result? _result;

            public JadwalUjianBuilder(JadwalUjian akurasiPenelitian)
            {
                _akurasiPenelitian = akurasiPenelitian;
            }

            private bool HasError => _result is not null && _result.IsFailure;

            public Result<JadwalUjian> Build()
            {
                if (!DateTime.TryParseExact(_akurasiPenelitian.Tanggal + " " + _akurasiPenelitian.JamMulai, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var mulai))
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.InvalidScheduleFormat("start"));
                }

                if (!DateTime.TryParseExact(_akurasiPenelitian.Tanggal + " " + _akurasiPenelitian.JamAkhir, "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var akhir))
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.InvalidScheduleFormat("end"));
                }

                if (_akurasiPenelitian.IdBankSoal <= 0)
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.IdBankSoalNotFound(_akurasiPenelitian.IdBankSoal));
                }

                if (_akurasiPenelitian.Kuota < -1)
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.KuotaInvalid());

                }
                return HasError ? Result.Failure<JadwalUjian>(_result!.Error) : Result.Success(_akurasiPenelitian);
            }

            public JadwalUjianBuilder ChangeDeskripsi(string? Deskripsi)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Deskripsi = Deskripsi;
                return this;
            }

            public JadwalUjianBuilder ChangeKuota(int Kuota)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Kuota = Kuota;
                return this;
            }

            public JadwalUjianBuilder ChangeTanggal(string Tanggal)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Tanggal = Tanggal;
                return this;
            }

            public JadwalUjianBuilder ChangeJamMulai(string JamMulai)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.JamMulai = JamMulai;
                return this;
            }

            public JadwalUjianBuilder ChangeJamAkhir(string JamAkhir)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.JamAkhir = JamAkhir;
                return this;
            }

            public JadwalUjianBuilder ChangeBankSoal(int IdBankSoal)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<JadwalUjian>(JadwalUjianErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.IdBankSoal = IdBankSoal;
                return this;
            }
        }
    }
}
