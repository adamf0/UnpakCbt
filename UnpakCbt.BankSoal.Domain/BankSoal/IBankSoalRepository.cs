using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.BankSoal.Domain.BankSoal
{
    public interface IBankSoalRepository
    {
        void Insert(BankSoal BankSoal);
        Task<BankSoal> GetAsync(Guid Uuid, CancellationToken cancellationToken = default);
        Task DeleteAsync(BankSoal BankSoal);
    }
}
