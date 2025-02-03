using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.BankSoal.Domain.BankSoal
{
    public sealed partial class BankSoal
    {
        public sealed class BankSoalBuilder
        {
            private readonly BankSoal _akurasiPenelitian;
            private Result? _result;

            public BankSoalBuilder(BankSoal akurasiPenelitian)
            {
                _akurasiPenelitian = akurasiPenelitian;
            }

            private bool HasError => _result is not null && _result.IsFailure;

            public Result<BankSoal> Build()
            {
                return HasError ? Result.Failure<BankSoal>(_result!.Error) : Result.Success(_akurasiPenelitian);
            }

            public BankSoalBuilder ChangeJudul(string judul)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<BankSoal>(BankSoalErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Judul = judul;
                return this;
            }

            public BankSoalBuilder ChangeRule(string? rule = null)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<BankSoal>(BankSoalErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Rule = rule;
                return this;
            }
        }
    }
}
