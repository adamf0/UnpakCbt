using Dapper;
using System.Data.Common;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Laporan.Domain.Laporan;
using UnpakCbt.Modules.Laporan.Application.Laporan.GetLaporanLulus;
using System.Data;
using System.Text;


namespace UnpakCbt.Modules.Laporan.Application.Laporan.GetAllLaporanLulus
{
    internal sealed class GetAllLaporanLulusQueryHandler(IDbConnectionFactory _dbConnectionFactory)
        : IQueryHandler<GetAllLaporanLulusQuery, List<LaporanLulusResponse>>
    {
        public async Task<Result<List<LaporanLulusResponse>>> Handle(GetAllLaporanLulusQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            // Tambahkan kondisi hanya jika nilainya valid
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
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("""
                SELECT 
                    u.uuid AS Uuid,
                    ju.uuid AS UuidJadwalUjian,
                    u.no_reg AS NoReg,
                    ju.tanggal AS Tanggal,
                    ju.jam_mulai_ujian AS JamMulai,
                    ju.jam_akhir_ujian AS JamAkhir,
                    ju.deskripsi AS Deskripsi,
                    u.keputusan AS Keputusan,
                    u.tanggal_respon AS TanggalRespon
                FROM ujian u
                JOIN jadwal_ujian ju ON u.id_jadwal_ujian = ju.id
            """);

            // Tambahkan WHERE hanya jika ada kondisi
            if (conditions.Any())
            {
                sqlBuilder.AppendLine("WHERE " + string.Join(" AND ", conditions));
            }

            string sql = sqlBuilder.ToString();

            var queryResult = await connection.QueryAsync<LaporanLulusResponse>(sql, parameters);

            if (!queryResult.Any())
            {
                return Result.Failure<List<LaporanLulusResponse>>(LaporanErrors.EmptyData());
            }

            return Result.Success(queryResult.ToList());
        }

    }
}
