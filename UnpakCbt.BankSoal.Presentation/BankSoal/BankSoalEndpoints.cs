using Microsoft.AspNetCore.Routing;

namespace UnpakCbt.Modules.BankSoal.Presentation.BankSoal
{
    public static class BankSoalEndpoints
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            CreateBankSoal.MapEndpoint(app);
            UpdateBankSoal.MapEndpoint(app);
            DeleteBankSoal.MapEndpoint(app);
            GetBankSoal.MapEndpoint(app);
            GetAllBankSoal.MapEndpoint(app);
        }
    }
}
