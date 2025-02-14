using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban
{
    public sealed partial class TemplateJawaban
    {
        public sealed class TemplateJawabanBuilder
        {
            private readonly TemplateJawaban _akurasiPenelitian;
            private Result? _result;

            public TemplateJawabanBuilder(TemplateJawaban akurasiPenelitian)
            {
                _akurasiPenelitian = akurasiPenelitian;
            }

            private bool HasError => _result is not null && _result.IsFailure;

            public Result<TemplateJawaban> Build()
            {
                if (_akurasiPenelitian.IdTemplateSoal <= 0)
                {
                    _result = Result.Failure<TemplateJawaban>(TemplateJawabanErrors.IdTemplateSoalNotFound(_akurasiPenelitian.IdTemplateSoal));
                }
                if (string.IsNullOrEmpty(_akurasiPenelitian.JawabanText) && string.IsNullOrEmpty(_akurasiPenelitian.JawabanImg))
                {
                    _result = Result.Failure<TemplateJawaban>(TemplateJawabanErrors.ImgTextNotEmpty());
                }

                return HasError ? Result.Failure<TemplateJawaban>(_result!.Error) : Result.Success(_akurasiPenelitian);
            }

            public TemplateJawabanBuilder ChangeTemplateSoal(int idTemplateSoal)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplateJawaban>(TemplateJawabanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.IdTemplateSoal = idTemplateSoal;

                return this;
            }

            public TemplateJawabanBuilder ChangeJawabanText(string? jawabanText)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplateJawaban>(TemplateJawabanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.JawabanText = jawabanText;

                return this;
            }

            public TemplateJawabanBuilder ChangeJawabanImg(string? jawabanImg)
            {
                if (HasError) return this;

                /*if (string.IsNullOrWhiteSpace(nama))
                {
                    _result = Result.Failure<TemplateJawaban>(TemplateJawabanErrors.NamaNotFound);
                    return this;
                }*/

                _akurasiPenelitian.JawabanImg = jawabanImg;

                return this;
            }
        }
    }
}
