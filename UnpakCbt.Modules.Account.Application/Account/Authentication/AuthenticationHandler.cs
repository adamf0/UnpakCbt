﻿using Dapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnpakCbt.Common.Application.Data;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Account.Application.Account.Authentication;
using UnpakCbt.Modules.Account.Application.Account.GetAccount;
using UnpakCbt.Modules.Account.Domain.Account;


namespace UnpakCbt.Modules.Account.Application.Authentication.Authentication
{
    internal sealed class AuthenticationHandler(IDbConnectionFactory _dbConnectionFactory)
        : IQueryHandler<AuthenticationQuery, string>
    {
        private string GenerateJwtToken(string uuid, string level)
        {
            string levelCode = level switch
            {
                "admin" => "a1f8",
                _ => "0000"
            };
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "localhost"),
                new Claim(JwtRegisteredClaimNames.Jti, $"{uuid}-{levelCode}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Key_Signed") ?? "UnpakCBTSecureKey1234567890!@$%#*&^"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("Issuer") ?? "localhost",
                audience: Environment.GetEnvironmentVariable("Audience") ?? "localhost",
                claims: claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Result<string>> Handle(AuthenticationQuery request, CancellationToken cancellationToken)
        {
            await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

            const string sql =
            """
            SELECT 
                CAST(NULLIF(uuid, '') AS VARCHAR(36)) AS Uuid,
                username as Username,
                level AS Level,
                status AS Status
            FROM user
            WHERE username = @Username and password = @Password
            """;

            var queryResult = await connection.QueryFirstOrDefaultAsync<AccountResponse>(sql, new { Username = request.username, Password = request.password });

            if (queryResult == null)
            {
                return Result.Failure<string>(AccountErrors.InvalidAuth());
            }

            return Result.Success(GenerateJwtToken(queryResult.Uuid, queryResult.Level));
        }
    }
}
