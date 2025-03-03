using System.Reflection.Emit;
using System.Text.RegularExpressions;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Account.Domain.Account
{
    public sealed partial class Account
    {
        public sealed class AccountBuilder
        {
            private readonly Account _akurasiPenelitian;
            private Result? _result;

            public AccountBuilder(Account akurasiPenelitian)
            {
                _akurasiPenelitian = akurasiPenelitian;
            }

            private bool HasError => _result is not null && _result.IsFailure;

            public Result<Account> Build()
            {
                string pattern1 = @"[!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?/\\]";
                //string pattern2 = @"^[0-9]+$";

                if (string.IsNullOrWhiteSpace(_akurasiPenelitian.Username))
                {
                    _result = Result.Failure<Account>(AccountErrors.EmptyUsername());
                }
                if (string.IsNullOrWhiteSpace(_akurasiPenelitian.Password))
                {
                    _result = Result.Failure<Account>(AccountErrors.EmptyPassword());
                }
                if (_akurasiPenelitian.Password.Length < 8)
                {
                    _result = Result.Failure<Account>(AccountErrors.MinPassword());
                }
                if (!Regex.IsMatch(_akurasiPenelitian.Password, pattern1))
                {
                    _result = Result.Failure<Account>(AccountErrors.SpecialCharcterPassword());
                }
                /*if (!Regex.IsMatch(_akurasiPenelitian.Password, pattern2))
                {
                    _result = Result.Failure<Account>(AccountErrors.NumberPassword());
                }*/
                if (string.IsNullOrWhiteSpace(_akurasiPenelitian.Level))
                {
                    _result = Result.Failure<Account>(AccountErrors.EmptyLevel());
                }
                if (_akurasiPenelitian.Level != "admin")
                {
                    _result = Result.Failure<Account>(AccountErrors.InvalidLevel(_akurasiPenelitian.Level));
                }

                return HasError ? Result.Failure<Account>(_result!.Error) : Result.Success(_akurasiPenelitian);
            }

            public AccountBuilder ChangeUsername(string username)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<Account>(AccountErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Username = username;
                return this;
            }

            public AccountBuilder ChangePassword(string? password = null)
            {
                if (HasError) return this;

                if (!string.IsNullOrWhiteSpace(password))
                {
                    _akurasiPenelitian.Password = password;
                }

                return this;
            }

            public AccountBuilder ChangeLevel(string level)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<Account>(AccountErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Level = level;
                return this;
            }

            public AccountBuilder ChangeStatus(string status)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<Account>(AccountErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.Status = status;
                return this;
            }
        }
    }
}
