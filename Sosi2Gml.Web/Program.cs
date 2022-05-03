using OSGeo.OGR;
using Sosi2Gml.Application.Mappers;
using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Mappers;
using Sosi2Gml.Reguleringsplanforslag.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddTransient<IGmlElementMapper<Identifikasjon>, IdentifikasjonMapper>();
services.AddTransient<IGmlElementMapper<Kvalitet>, KvalitetMapper>();

services.AddTransient<IGmlFeatureMapper<RpGrense>, RpGrenseMapper>();
services.AddTransient<IGmlFeatureMapper<RpForm�lGrense>, RpForm�lGrenseMapper>();

services.AddTransient<IGmlSurfaceFeatureMapper<RpOmr�de, RpGrense>, RpOmr�deMapper>();
services.AddTransient<IGmlSurfaceFeatureMapper<RpArealform�lOmr�de, RpForm�lGrense>, RpArealform�lOmr�deMapper>();



Ogr.RegisterAll();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
