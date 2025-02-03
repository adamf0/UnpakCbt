using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.PertanyaanImg = pertanyaanImg;

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
