using Microsoft.AspNetCore.Routing;

namespace UnpakCbt.Modules.Account.Presentation.Account
{
    public static class AccountEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateAccount.MapEndpoint(app);
            UpdateAccount.MapEndpoint(app);
            StatusAccount.MapEndpoint(app);
            DeleteAccount.MapEndpoint(app);
            Authentication.MapEndpoint(app);
            GetAccount.MapEndpoint(app);
            GetAllAccount.MapEndpoint(app);
            GetAllAccountWithPaging.MapEndpoint(app);
        }
    }
}
