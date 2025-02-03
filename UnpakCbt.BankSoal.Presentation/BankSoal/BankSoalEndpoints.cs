using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
