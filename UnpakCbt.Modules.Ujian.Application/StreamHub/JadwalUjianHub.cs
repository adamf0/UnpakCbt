using Microsoft.AspNetCore.SignalR;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;

namespace UnpakCbt.Modules.Ujian.Application.StreamHub
{
    public class JadwalUjianHub : Hub
    {
        public async Task SendJadwalUjianUpdate(UjianDetailResponse respons)
        {
            await Clients.All.SendAsync("ReceiveJadwalUjianUpdate", respons);
        }
    }
}
