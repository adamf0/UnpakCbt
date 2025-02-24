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
using UnpakCbt.Api.Extensions;
using UnpakCbt.Api.Security;
using System.Runtime.CompilerServices;

//[::]:5000/swagger/index.html

var builder = WebApplication.CreateBuilder(args);
RuntimeFeature.IsDynamicCodeSupported.Equals(false);
RuntimeFeature.IsDynamicCodeCompiled.Equals(false);
AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", false);



builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
    //loggerConfig.WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? "https://host.docker.internal:5341");
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
    c.DocumentFilter<SwaggerAddApiPrefixDocumentFilter>();

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

SecurityConfig.PreventDynamicCodeExecution();

var app = builder.Build();
app.UseUserAgentMiddleware();
app.UseSecurityHeadersMiddleware();

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

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads"
});
//app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseAuthorization();
app.MapControllers();

app.Run();
