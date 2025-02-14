﻿using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan
{
    public static class TemplatePertanyaanErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("TemplatePertanyaan.EmptyData", "Data is not found");

        public static Error NotFound(Guid Id) =>
            Error.Problem("TemplatePertanyaan.NotFound", $"Question with identifier {Id} not found");

        public static Error IdBankSoalNotFound(Guid IdBankSoal) =>
            Error.Problem("TemplatePertanyaan.IdBankSoalNotFound", $"Questions with reference IdBankSoal {IdBankSoal} not found");

        public static Error IdBankSoalNotFound(int IdBankSoal) =>
            Error.Problem("TemplatePertanyaan.IdBankSoalNotFound", $"Questions with reference IdBankSoal {IdBankSoal} (int) not found");

        public static Error TipeNotFound(string Tipe) =>
            Error.Problem("TemplatePertanyaan.TipeNotFound", $"Questions with reference Tipe {Tipe} not found");

        public static Error ImgTextNotEmpty() =>
            Error.Problem("TemplatePertanyaan.ImgTextNotEmpty", "Image or text question references in the question cannot be empty");

        public static Error JawabanBenarNotEmpty() =>
            Error.Problem("TemplatePertanyaan.JawabanBenarNotEmpty", "Correct answer question references in the question not found");

        public static Error BobotNotEmpty() =>
            Error.Problem("TemplatePertanyaan.BobotNotEmpty", "Weight question references in the question not found");
    }
}
