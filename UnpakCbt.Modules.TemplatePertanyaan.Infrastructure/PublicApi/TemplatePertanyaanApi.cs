using MediatR;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;
using ITemplatePertanyaanApi = UnpakCbt.Modules.TemplatePertanyaan.PublicApi.ITemplatePertanyaanApi;
using TemplatePertanyaanResponseApi = UnpakCbt.Modules.TemplatePertanyaan.PublicApi.TemplatePertanyaanResponse;

namespace UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.PublicApi
{
    internal sealed class TemplatePertanyaanApi(ISender sender) : ITemplatePertanyaanApi
    {
        public async Task<List<TemplatePertanyaanResponseApi>> GetAllTemplatePertanyaanByBankSoal(int IdBankSoal, CancellationToken cancellationToken = default)
        {
            Result<List<TemplatePertanyaanDefaultResponse>> result = await sender.Send(new GetAllTemplatePertanyaanDefaultByBankSoalQuery(IdBankSoal), cancellationToken);

            if (result.IsFailure)
            {
                return new List<TemplatePertanyaanResponseApi>();
            }

            return result.Value.Select(item => new TemplatePertanyaanResponseApi(
                item.Id,
                item.Uuid,
                int.Parse(item.IdBankSoal),
                item.Tipe,
                item.Pertanyaan,
                item.Gambar,
                item.JawabanBenar == null ? null : int.Parse(item.JawabanBenar),
                item.Bobot == null ? null : int.Parse(item.Bobot),
                item.State
            )).ToList();
        }

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
