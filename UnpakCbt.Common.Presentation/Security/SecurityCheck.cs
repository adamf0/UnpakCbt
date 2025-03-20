using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace UnpakCbt.Common.Presentation.Security
{
    public class SecurityCheck
    {
        private static readonly Regex InvalidCharactersRegex = new(
            @"[\p{Cf}\p{Cn}\p{Co}\u200B\u200C\u200D\u2060]", // Zero-width spaces dan karakter kontrol
            RegexOptions.Compiled);

        private static readonly Regex HomoglyphRegex = new(
            "[𝟘𝟢🄀Ｏ⓪🯰𝟙𝟣🄁𝟏⓵１𝟚𝟤🄂𝟐⓶２𝟛𝟥🄃𝟑⓷３𝟜𝟦🄄𝟒⓸４𝟝𝟧🄅𝟓⓹５𝟞𝟨🄆𝟔⓺６𝟟𝟩🄇𝟕⓻７𝟠𝟪🄈𝟖⓼８𝟡𝟫🄉𝟗⓽９˗‐‑‒–—―а𝖆𝒂𝓪𝗮𝚊]",
            RegexOptions.Compiled);

        private static readonly Regex GuidRegex = new Regex(
        "^[{(]?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}[)}]?$",
        RegexOptions.Compiled);

        public static bool NotContainInvalidCharacters(string guid)
        {
            string guidStr = guid.ToString();
            return (!InvalidCharactersRegex.IsMatch(guidStr) && !HomoglyphRegex.IsMatch(guidStr)) || guid.IsNullOrEmpty();
        }

        public static bool isValidGuid(string guid) {
            return GuidRegex.IsMatch(guid);
        }
    }
}
