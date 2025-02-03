using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Common.Domain
{
    public interface IDomainEvent
    {
        Guid Id { get; }

        DateTime OccurredOnUtc { get; }
    }
}
