using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal;
using IBankSoalApi = UnpakCbt.Modules.BankSoal.PublicApi.IBankSoalApi;
using BankSoalResponseApi = UnpakCbt.Modules.BankSoal.PublicApi.BankSoalResponse;

namespace UnpakCbt.Modules.BankSoal.Infrastructure.PublicApi
{
    internal sealed class BankSoalApi(ISender sender) : IBankSoalApi
    {
        public async Task<BankSoalResponseApi?> GetAsync(Guid BankSoalUuid, CancellationToken cancellationToken = default)
        {
            Result<BankSoalDefaultResponse> result = await sender.Send(new GetBankSoalDefaultQuery(BankSoalUuid), cancellationToken);

            if (result.IsFailure)
            {
                return null;
            }

            return new BankSoalResponseApi(
                result.Value.Id,
                result.Value.Uuid,
                result.Value.Judul,
                result.Value.Rule
            );
        }
    }
}
