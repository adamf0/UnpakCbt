using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Laporan.Domain.Laporan;
using System.Data;
using System.Text;

namespace UnpakCbt.Modules.Laporan.Application.Laporan.GetAllLaporanLulusTotal
{
    internal sealed class GetAllLaporanLulusTotalQueryHandler(IDbConnectionFactory _dbConnectionFactory)
        : IQueryHandler<GetAllLaporanLulusTotalQuery, LaporanLulusTotalResponse>
    {
        public async Task<Result<LaporanLulusTotalResponse>> Handle(GetAllLaporanLulusTotalQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(request.UuidJadwalUjian))
            {
                conditions.Add("ju.uuid = @Uuid");
                parameters.Add("@Uuid", request.UuidJadwalUjian, DbType.String);
            }

            if (!string.IsNullOrEmpty(request.TanggalMulai) && !string.IsNullOrEmpty(request.TanggalAkhir))
            {
                conditions.Add("ju.tanggal BETWEEN @TanggalMulai AND @TanggalAkhir");
                parameters.Add("@TanggalMulai", request.TanggalMulai, DbType.Date);
                parameters.Add("@TanggalAkhir", request.TanggalAkhir, DbType.Date);
            }

            // Gunakan StringBuilder untuk membangun query dengan lebih aman
            StringBuilder sqlBuilder = new();
            sqlBuilder.AppendLine("""
                SELECT
                    COUNT(CASE WHEN u.keputusan = 'tidak lulus' THEN 1 END) AS TotalTidakLulus,
                    COUNT(CASE WHEN u.keputusan = 'lulus' THEN 1 END) AS TotalLulus,
                    COUNT(CASE WHEN u.keputusan = 'lulus' AND u.tanggal_respon IS NOT NULL THEN 1 END) AS TotalLulusDenganRespon
                FROM ujian u
                JOIN jadwal_ujian ju ON u.id_jadwal_ujian = ju.id
            """);

            if (conditions.Any())
            {
                sqlBuilder.AppendLine("WHERE " + string.Join(" AND ", conditions));
            }

            string sql = sqlBuilder.ToString();

            var queryResult = await connection.QuerySingleOrDefaultAsync<LaporanLulusTotalResponse>(sql, parameters);

            if (queryResult == null)
            {
                return Result.Failure<LaporanLulusTotalResponse>(LaporanErrors.EmptyData());
            }

            return queryResult;
        }
    }
}
