﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Account.Application.Account.Authentication;

namespace UnpakCbt.Modules.Account.Presentation.Account
{
    internal static class Authentication
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("Authentication", async (AuthenticationRequest request, ISender sender) =>
            {
                //[PR] anti xss
                Result<string> result = await sender.Send(new AuthenticationQuery(request.Username, request.Password));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Authentication);
        }
    }

    internal sealed class AuthenticationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
