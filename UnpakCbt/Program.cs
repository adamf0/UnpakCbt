using Serilog;
using UnpakCbt.Common.Application;
using UnpakCbt.Common.Infrastructure;
using UnpakCbt.Api.Middleware;
using UnpakCbt.Modules.BankSoal.Infrastructure;
using UnpakCbt.Modules.TemplatePertanyaan.Infrastructure;
using UnpakCbt.Modules.TemplateJawaban.Infrastructure;
using UnpakCbt.Modules.JadwalUjian.Infrastructure;
using UnpakCbt.Modules.Ujian.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using UnpakCbt.Api;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;

//[::]:5000/swagger/index.html

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
    loggerConfig.WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "https://host.docker.internal:5341");
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new IgnoreAntiforgeryTokenAttribute()); // Abaikan antiforgery untuk semua request
});
builder.Services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

//builder.Configuration.AddModuleConfiguration(["Asset"]);
builder.Services.AddApplication([
    UnpakCbt.Modules.BankSoal.Application.AssemblyReference.Assembly,
    UnpakCbt.Modules.TemplatePertanyaan.Application.AssemblyReference.Assembly,
    UnpakCbt.Modules.TemplateJawaban.Application.AssemblyReference.Assembly,
    UnpakCbt.Modules.JadwalUjian.Application.AssemblyReference.Assembly,
    UnpakCbt.Modules.Ujian.Application.AssemblyReference.Assembly,
]);
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; // Token harus dikirim melalui header ini
    options.Cookie.Name = "XSRF-TOKEN"; // Nama cookie antiforgery
});

builder.Services.AddInfrastructure(
     Environment.GetEnvironmentVariable("ConnectionStrings__Database") ?? builder.Configuration.GetConnectionString("Database")
);
builder.Services.AddBankSoalModule(builder.Configuration);
builder.Services.AddTemplatePertanyaanModule(builder.Configuration);
builder.Services.AddTemplateJawabanModule(builder.Configuration);
builder.Services.AddJadwalUjianModule(builder.Configuration);
builder.Services.AddUjianModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UnpakCbt API",
        Version = "v1"
    });
    //c.DocumentFilter<SwaggerAddApiPrefixDocumentFilter>();

    c.OperationFilter<SwaggerFileOperationFilter>();
});
/*builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllOrigins",
        configurePolicy: policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});*/
builder.Services.AddAuthorization();
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});

var app = builder.Build();
BankSoalModule.MapEndpoints(app);
TemplatePertanyaanModule.MapEndpoints(app);
TemplateJawabanModule.MapEndpoints(app);
JadwalUjianModule.MapEndpoints(app);
UjianModule.MapEndpoints(app);

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseStaticFiles(); // Menyajikan file dari wwwroot secara umum
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads"
});
//app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAntiforgery();

app.Use((context, next) =>
{
    var userAgent = context.Request.Headers.UserAgent.ToString();

    if (string.IsNullOrEmpty(userAgent))
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Bad Request",
            Detail = "Unknown User-Agent"
        };
        context.Response.StatusCode = problemDetails.Status.Value;

        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsJsonAsync(problemDetails);
    }

    return next();
});

app.Use((context, next) =>
{
    context.Response.Headers.Remove("X-AspNet-Version");
    context.Response.Headers["X-DNS-Prefetch-Control"] = "off";

    context.Response.Headers.XFrameOptions = "DENY";
    context.Response.Headers.XXSSProtection = "1; mode=block";
    context.Response.Headers.XContentTypeOptions = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers.ContentType = "application/json; charset=UTF-8";
    context.Response.Headers.StrictTransportSecurity = "max-age=60; includeSubDomains; preload";
    context.Response.Headers.AccessControlAllowOrigin = Environment.GetEnvironmentVariable("Mode")=="prod"? "https://host.docker.internal" : "https://localhost";
    context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin";
    context.Response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp";
    context.Response.Headers["Cross-Origin-Resource-Policy"] = "same-site";

    context.Response.Headers.Remove("X-Powered-By");
    return next();
});
app.UseAuthorization();
app.MapControllers();

app.Run();
