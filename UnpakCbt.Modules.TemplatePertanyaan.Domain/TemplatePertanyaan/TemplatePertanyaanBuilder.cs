using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan
{
    public sealed partial class TemplatePertanyaan
    {
        public sealed class TemplatePertanyaanBuilder
        {
            private readonly TemplatePertanyaan _akurasiPenelitian;
            private Result? _result;

            public TemplatePertanyaanBuilder(TemplatePertanyaan akurasiPenelitian)
            {
                _akurasiPenelitian = akurasiPenelitian;
            }

            private bool HasError => _result is not null && _result.IsFailure;

            public Result<TemplatePertanyaan> Build()
            {
                if (_akurasiPenelitian.IdBankSoal <= 0)
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.IdBankSoalNotFound(_akurasiPenelitian.IdBankSoal));
                }
                if (string.IsNullOrWhiteSpace(_akurasiPenelitian.Tipe))
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.TipeNotFound(_akurasiPenelitian.Tipe));
                }
                if (string.IsNullOrEmpty(_akurasiPenelitian.PertanyaanText) && string.IsNullOrEmpty(_akurasiPenelitian.PertanyaanImg))
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.ImgTextNotEmpty());
                }
                if (_akurasiPenelitian.JawabanBenar == null || _akurasiPenelitian.JawabanBenar <= 0)
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.JawabanBenarNotEmpty());
                }
                if (_akurasiPenelitian.Bobot == null || _akurasiPenelitian.Bobot <= 0)
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.BobotNotEmpty());
                }

                return HasError ? Result.Failure<TemplatePertanyaan>(_result!.Error) : Result.Success(_akurasiPenelitian);
            }

            public TemplatePertanyaanBuilder ChangeBankSoal(int idBankSoal)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.IdBankSoal = idBankSoal;

                return this;
            }

            public TemplatePertanyaanBuilder ChangeTipe(string tipe)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Tipe = tipe;

                return this;
            }

            public TemplatePertanyaanBuilder ChangePertanyaanText(string? pertanyaanText)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.PertanyaanText = pertanyaanText;

                return this;
            }

            public TemplatePertanyaanBuilder ChangePertanyaanImg(string? pertanyaanImg)
            {
                if (HasError) return this;

                //if (!string.IsNullOrWhiteSpace(pertanyaanImg))
                //{
                    _akurasiPenelitian.PertanyaanImg = pertanyaanImg;
                //}

                return this;
            }

            public TemplatePertanyaanBuilder ChangeJawabanBenar(int? jawabanBenar)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.JawabanBenar = jawabanBenar;

                return this;
            }

            public TemplatePertanyaanBuilder ChangeBobot(int? bobot)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Bobot = bobot;

                return this;
            }

            public TemplatePertanyaanBuilder ChangeState(string? state)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.State = state;

                return this;
            }
        }
    }
}
