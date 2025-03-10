using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Account.Domain.Account
{
    public static class AccountErrors
    {
        public static Error InvalidPage() =>
            Error.Problem("Account.InvalidPage", "Page minimum is 1");
        public static Error InvalidPageSize() =>
            Error.Problem("Account.InvalidPageSize", "Page size minimun is 1");
        public static Error InvalidSearchRegistry(string value) =>
            Error.Problem("Account.InvalidSearchRegistry", $"Search column {value} not registered in system");
        public static Error InvalidSortRegistry(string value) =>
            Error.Problem("Account.InvalidSortRegistry", $"Sort column {value} not registered in system");
        public static Error InvalidArgs(string value) =>
           Error.Problem("Account.InvalidArgs", value);

        public static Error EmptyData() =>
            Error.NotFound("Account.EmptyData", "Data is not found");

        public static Error NotFound(Guid Id) =>
            Error.Problem("Account.NotFound", $"Account with identifier {Id} not found");

        public static Error NotUnique(string Username) =>
           Error.Problem("Account.NotUnique", $"Account username {Username} is not unique");

        public static Error InvalidAuth() =>
            Error.Problem("Account.InvalidAuth", "Failed in Authentication due to incorrect username or password");

        public static Error EmptyUsername() =>
            Error.Problem("Account.EmptyUsername", "Account field username is required");
        public static Error EmptyPassword() =>
            Error.Problem("Account.EmptyPassword", "Account field password is required");
        public static Error MinPassword() =>
            Error.Problem("Account.MinPassword", "Account field password must be at least 8 characters");
        public static Error SpecialCharcterPassword() =>
            Error.Problem("Account.SpecialCharcterPassword", "Account field password must contain special characters");
        public static Error NumberPassword() =>
            Error.Problem("Account.NumberPassword", "Account field password must contain number");

        public static Error EmptyLevel() =>
            Error.Problem("Account.EmptyLevel", "Account field level is required");

        public static Error InvalidLevel(string level) =>
            Error.Problem("Account.EmptyLevel", $"Account field level value {level} is not accepted in the system");

        public static Error InvalidPassword() =>
            Error.Problem("Account.InvalidPassword", "Account found but wrong password");
    }
}
