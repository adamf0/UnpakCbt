using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public sealed partial class Ujian
    {
        public sealed class UjianBuilder
        {
            private readonly Ujian _akurasiPenelitian;
            private Result? _result;

            public UjianBuilder(Ujian akurasiPenelitian)
            {
                _akurasiPenelitian = akurasiPenelitian;
            }

            private bool HasError => _result is not null && _result.IsFailure;

            public Result<Ujian> Build()
            {
                if (string.IsNullOrWhiteSpace(_akurasiPenelitian.NoReg))
                {
                    _result = Result.Failure<Ujian>(UjianErrors.NoRegNotEmpty());
                }
                if (_akurasiPenelitian.IdJadwalUjian <= 0)
                {
                    _result = Result.Failure<Ujian>(UjianErrors.NoRegNotEmpty());

                }
                return HasError ? Result.Failure<Ujian>(_result!.Error) : Result.Success(_akurasiPenelitian);
            }

            public UjianBuilder ChangeNoReg(string NoReg)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<Ujian>(UjianErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.NoReg = NoReg;
                return this;
            }

            public UjianBuilder ChangeJadwalUjian(int IdJadwalUjian)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<Ujian>(UjianErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.IdJadwalUjian = IdJadwalUjian;
                return this;
            }

            public UjianBuilder ChangeStatus(string Status)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<Ujian>(UjianErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Status = Status;
                return this;
            }
        }
    }
}
