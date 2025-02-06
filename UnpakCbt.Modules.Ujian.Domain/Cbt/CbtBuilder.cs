using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public sealed partial class Cbt
    {
        public sealed class CbtBuilder
        {
            private readonly Cbt _akurasiPenelitian;
            private Result? _result;

            public CbtBuilder(Cbt akurasiPenelitian)
            {
                _akurasiPenelitian = akurasiPenelitian;
            }

            private bool HasError => _result is not null && _result.IsFailure;

            public Result<Cbt> Build()
            {
                return HasError ? Result.Failure<Cbt>(_result!.Error) : Result.Success(_akurasiPenelitian);
            }

            public CbtBuilder ChangeUjian(int IdUjian)
            {
                if (HasError) return this;

                _akurasiPenelitian.IdUjian = IdUjian;
                return this;
            }

            public CbtBuilder ChangeTemplateSoal(int IdTemplateSoal)
            {
                if (HasError) return this;

                _akurasiPenelitian.IdTemplateSoal = IdTemplateSoal;
                return this;
            }

            public CbtBuilder ChangeJawabanBenar(int JawabanBenar)
            {
                if (HasError) return this;

                _akurasiPenelitian.JawabanBenar = JawabanBenar;
                return this;
            }

        }
    }
}
