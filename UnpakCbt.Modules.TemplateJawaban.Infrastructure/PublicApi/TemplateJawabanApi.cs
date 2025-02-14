using MediatR;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban;
using ITemplateJawabanApi = UnpakCbt.Modules.TemplateJawaban.PublicApi.ITemplateJawabanApi;
using TemplateJawabanResponseApi = UnpakCbt.Modules.TemplateJawaban.PublicApi.TemplateJawabanResponse;

namespace UnpakCbt.Modules.TemplateJawaban.Infrastructure.PublicApi
{
    internal sealed class TemplateJawabanApi(ISender sender) : ITemplateJawabanApi
    {
        public async Task<TemplateJawabanResponseApi?> GetAsync(Guid TemplateJawabanUuid, CancellationToken cancellationToken = default)
        {
            Result<TemplateJawabanDefaultResponse> result = await sender.Send(new GetTemplateJawabanDefaultQuery(TemplateJawabanUuid), cancellationToken);

            if (result.IsFailure)
            {
                return null;
            }

            return new TemplateJawabanResponseApi(
                result.Value.Id,
                result.Value.Uuid,
                result.Value.JawabanText,
                result.Value.JawabanImg
            );
        }
    }
}
