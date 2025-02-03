﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Common.Application.Messaging
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
}
