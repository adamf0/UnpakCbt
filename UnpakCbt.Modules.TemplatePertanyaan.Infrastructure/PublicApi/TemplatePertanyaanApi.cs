using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;
using ITemplatePertanyaanApi = UnpakCbt.Modules.TemplatePertanyaan.PublicApi.ITemplatePertanyaanApi;
using TemplatePertanyaanResponseApi = UnpakCbt.Modules.TemplatePertanyaan.PublicApi.TemplatePertanyaanResponse;

namespace UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.PublicApi
{
    internal sealed class TemplatePertanyaanApi(ISender sender) : ITemplatePertanyaanApi
    {
        public async Task<TemplatePertanyaanResponseApi?> GetAsync(Guid TemplatePertanyaanUuid, CancellationToken cancellationToken = default)
        {
            Result<TemplatePertanyaanDefaultResponse> result = await sender.Send(new GetTemplatePertanyaanDefaultQuery(TemplatePertanyaanUuid), cancellationToken);

            if (result.IsFailure)
            {
                return null;
            }

            return new TemplatePertanyaanResponseApi(
                result.Value.Id,
                result.Value.Uuid,
                int.Parse(result.Value.IdBankSoal),
                result.Value.Tipe,
                result.Value.Pertanyaan,
                result.Value.Gambar,
                result.Value.JawabanBenar==null? null:int.Parse(result.Value.JawabanBenar),
                result.Value.Bobot == null ? null : int.Parse(result.Value.Bobot),
                result.Value.State
            );
        }
    }
}
