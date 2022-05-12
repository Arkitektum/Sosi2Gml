using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using OSGeo.OGR;
using Serilog;
using Sosi2Gml.Application.Mappers;
using Sosi2Gml.Application.Services.MultipartRequest;
using Sosi2Gml.Reguleringsplanforslag.Configuration;
using Sosi2Gml.Web.Configuration;
using Sosi2Gml.Web.Middleware;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});

services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = new[] { "application/xml+gml" };
    options.Providers.Add<GzipCompressionProvider>();
});

services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "SOSI2GML", Version = "v1" });
    options.OperationFilter<MultipartOperationFilter>();
});

services.AddHttpContextAccessor();
services.AddTransient<IMultipartRequestService, MultipartRequestService>();
services.AddTransient<IGmlMapper, GmlMapper>();

services.AddApplicationServicesForReguleringsplanforslag();

services.ConfigureApplication(configuration);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Logging.AddSerilog(Log.Logger, true);

Ogr.RegisterAll();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());

app.UseResponseCompression();

app.UseMiddleware<SerilogMiddleware>();

app.Use(async (context, next) => {
    context.Request.EnableBuffering();
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
