using Dapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Application.StreamHub;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.AllDoneUjian
{
    internal sealed class AllDoneUjianCommandHandler(
    IHubContext<JadwalUjianHub> _hubContext,
    IDbConnectionFactory _dbConnectionFactory,
    ILogger<AllDoneUjianCommand> logger)
    : ICommandHandler<AllDoneUjianCommand>
    {
        public async Task<Result> Handle(AllDoneUjianCommand request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
            select 
            u.uuid as Uuid,
            ju.uuid as UuidJadwalUjian,
            u.no_reg as NoReg,
            u.status as Status 
            from ujian u
            join jadwal_ujian ju on u.id_jadwal_ujian = ju.id 
            where NOW() > concat(ju.tanggal, " ", ju.jam_akhir_ujian) and u.status != "done"
            """;

            const string sqlUpdate =
            """
            UPDATE ujian u
            JOIN jadwal_ujian ju ON u.id_jadwal_ujian = ju.id
            SET u.status = 'done'
            WHERE 
                NOW() > CONCAT(ju.tanggal, ' ', ju.jam_akhir_ujian)
                AND u.status != 'done';
            """;

            var queryResult = await connection.QueryAsync<UjianResponse>(sql);

            /*if (!queryResult.Any())
            {
                return Result.Failure<List<UjianResponse>>(UjianErrors.EmptyData());
            }*/

            if (queryResult.ToList().Count>0) {
                int affectedRows = await connection.ExecuteAsync(sqlUpdate);
                if (affectedRows > 0)
                {
                    logger.LogInformation("berhasil update status ke done");
                    foreach (var row in queryResult.ToList())
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveJadwalUjianUpdate", new UjianDetailResponse
                        {
                            Uuid = row.UuidJadwalUjian,
                            NoReg = row.NoReg,
                            Status = "done"
                        });
                        logger.LogInformation($"notify ui dengan data ujian {row.UuidJadwalUjian} no reg {row.NoReg}");
                    }
                }

            }


            return Result.Success();
        }
    }
}
