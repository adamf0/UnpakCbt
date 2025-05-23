﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Globalization;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.PublicApi;
using UnpakCbt.Modules.TemplatePertanyaan.PublicApi;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Application.StreamHub;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;
using UnpakCbt.Modules.Ujian.Domain.Cbt;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.CreateUjian
{
    internal sealed class CreateUjianCommandHandler(
        IHubContext<JadwalUjianHub> _hubContext,
        Domain.Ujian.ICounterRepository counterRepository,
        IUjianRepository ujianRepository,
        ICbtRepository cbtRepository,
        IUnitOfWork unitOfWork,
        IJadwalUjianApi jadwalUjianApi,
        ITemplatePertanyaanApi templatePertanyaanApi,
        ILogger<CreateUjianCommand> logger)
        : ICommandHandler<CreateUjianCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateUjianCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received command with parameters: {@Request}", request);

            int totalActiveUjian = await ujianRepository.GetCountJadwalActiveAsync(request.NoReg); //active, start, done
            if (totalActiveUjian > 0) {
                return Result.Failure<Guid>(UjianErrors.ActiveExam(request.NoReg));
            }

            JadwalUjianResponse? jadwalUjian = await jadwalUjianApi.GetAsync(request.IdJadwalUjian, cancellationToken);
            logger.LogInformation("jadwalUjian: {@jadwalUjian}", jadwalUjian);
            checkData(jadwalUjian, request.IdJadwalUjian);
            checkDataDate(jadwalUjian);
            checkFormatAndRangeDate(jadwalUjian);

            List<TemplatePertanyaanResponse> listMasterPertanyaanTrial = [];

            int enableTrial = 0;
            string enableTrialValue = Environment.GetEnvironmentVariable("EnableTrial");
            if (int.TryParse(enableTrialValue, out enableTrial) == false)
            {
                enableTrial = 0; // Default value
            }

            if (enableTrial==1) {
                logger.LogInformation("pengaturan trial ditemukan");
                string idBankSoalTrialValue = Environment.GetEnvironmentVariable("IdBankSoalTrial");
                int idBankSoalTrial = 0;
                if (int.TryParse(idBankSoalTrialValue, out idBankSoalTrial) == false)
                {
                    idBankSoalTrial = 0; // Default value
                }

                if (idBankSoalTrial == 0) {
                    logger.LogError("referensi template soal trial tidak ditemukan");
                    return Result.Failure<Guid>(UjianErrors.NotFoundQuestionTrial());
                }
                logger.LogInformation("referensi template soal trial ditemukan");

                listMasterPertanyaanTrial = await templatePertanyaanApi.GetAllTemplatePertanyaanByBankSoal(idBankSoalTrial);
            }
            

            //insert ujian
            var result = Domain.Ujian.Ujian.Create(request.NoReg, int.Parse(jadwalUjian?.Id??"0"));
            logger.LogInformation("result: {@result}", result);
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }
            ujianRepository.Insert(result.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //insert list pertanyaan
            Domain.Ujian.Ujian? currentUjian = await ujianRepository.GetAsync(result.Value.Uuid, cancellationToken);
            logger.LogInformation("currentUjian: {@currentUjian}", currentUjian);

            if (currentUjian.Id == null) {
                return Result.Failure<Guid>(UjianErrors.NotFoundReference());
            }

            List<TemplatePertanyaanResponse> listMasterPertanyaan = await templatePertanyaanApi.GetAllTemplatePertanyaanByBankSoal(jadwalUjian.IdBankSoal);
            IEnumerable<Domain.Cbt.Cbt> listPertanyaan = listMasterPertanyaan.Select(item =>
                Domain.Cbt.Cbt.Create(
                    currentUjian?.Id ?? 0,
                    int.Parse(item.Id)
                ).Value
            );
            await cbtRepository.InsertAsync(listPertanyaan);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (listMasterPertanyaanTrial.Count() > 0) {
                logger.LogInformation("template soal trial ditemukan");
                IEnumerable<Domain.Cbt.Cbt> listPertanyaanTrial = listMasterPertanyaanTrial.Select(item =>
                    Domain.Cbt.Cbt.CreateTrial(
                        currentUjian?.Id ?? 0,
                        int.Parse(item.Id)
                    ).Value
                );
                await cbtRepository.InsertAsync(listPertanyaanTrial);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            try { 
                await _hubContext.Clients.All.SendAsync("ReceiveJadwalUjianUpdate", new UjianDetailResponse
                {
                    Uuid = result.Value.Uuid.ToString(),
                    NoReg = request.NoReg,
                    Status = "active"
                });
            }
            catch (HubException hubEx)
            {
                logger.LogError($"SignalR error: {hubEx.Message}");
            }
            catch (TaskCanceledException taskEx)
            {
                logger.LogError($"SignalR request dibatalkan: {taskEx.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected error: {ex.Message}");
            }

            //add counter
            string key = "counter_" + request.IdJadwalUjian.ToString();
            bool checkKey = await counterRepository.KeyExistsAsync(key);
            int kuotaNewJadwal = (jadwalUjian?.Kuota ?? 0);
            int totalData = await ujianRepository.GetCountJadwalByJadwalUjianAsync(int.Parse(jadwalUjian?.Id ?? "0"));

            if (jadwalUjian.Kuota > 0)
            {
                int postCounter = await counterRepository.GetCounterAsync(key) + 1;
                if (totalData > kuotaNewJadwal)
                { //check total tabel
                    return Result.Failure<Guid>(UjianErrors.QuotaExhausted2(postCounter.ToString(), kuotaNewJadwal.ToString()));
                }
                if (checkKey && postCounter > jadwalUjian.Kuota)
                { //check total data redis
                    return Result.Failure<Guid>(UjianErrors.QuotaExhausted(postCounter.ToString(), jadwalUjian.Kuota.ToString()));
                }
            }
            if (checkKey) {
                await counterRepository.IncrementCounterAsync(key, null);
            }

            //tahap 2: insert ujian_cbt background job

            return result.Value.Uuid;
        }

        private Result? checkData(JadwalUjianResponse? jadwalUjian, Guid IdJadwalUjian) {
            if (jadwalUjian is null)
            {
                return Result.Failure<Guid>(Domain.JadwalUjian.JadwalUjianErrors.NotFound(IdJadwalUjian));
            }

            return null;
        }
        private Result? checkDataDate(JadwalUjianResponse? jadwalUjian) {
            if (string.IsNullOrWhiteSpace(jadwalUjian?.Tanggal) ||
                    string.IsNullOrWhiteSpace(jadwalUjian.JamMulai) ||
                    string.IsNullOrWhiteSpace(jadwalUjian.JamAkhir))
            {
                return Result.Failure<Guid>(UjianErrors.EmptyDataScheduleFormat());
            }

            return null;
        }
        private Result? checkFormatAndRangeDate(JadwalUjianResponse? jadwalUjian) {
            if (!DateTime.TryParseExact(jadwalUjian.Tanggal + " " + jadwalUjian.JamMulai, "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var mulai))
            {
                return Result.Failure<Guid>(UjianErrors.InvalidScheduleFormat("start"));
            }

            if (!DateTime.TryParseExact(jadwalUjian.Tanggal + " " + jadwalUjian.JamAkhir, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var akhir))
            {
                return Result.Failure<Guid>(UjianErrors.InvalidScheduleFormat("end"));
            }

            var sekarang = DateTime.UtcNow;
            if (mulai > akhir)
            {
                return Result.Failure<Guid>(UjianErrors.InvalidRangeDateTime());
            }

            if (sekarang >= mulai && sekarang <= akhir)
            {
                return Result.Failure<Guid>(UjianErrors.OutRange(mulai.ToString("yyyy-MM-dd HH:mm"), akhir.ToString("yyyy-MM-dd HH:mm")));
            }

            return null;
        }
    }
}
