﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public interface IUjianRepository
    {
        void Insert(Ujian Ujian);
        Task<Ujian> GetAsync(Guid Uuid, CancellationToken cancellationToken = default);
        Task DeleteAsync(Ujian Ujian);
    }
}
