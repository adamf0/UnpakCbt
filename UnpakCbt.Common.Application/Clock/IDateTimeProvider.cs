using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Common.Application.Clock
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}
