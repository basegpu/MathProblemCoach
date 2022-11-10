using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using MathProblem.API.Models;
using MathProblem.API.SecretSauce;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JsonOptions>(opt =>
{
    opt.SerializerOptions.WriteIndented = true;
    opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddRazorPages();
builder.Services.AddEndpointDefinitions(typeof(GeneratorConfig), typeof(OpenApiInfo));

var app = builder.Build();

app.UseEndpointDefinitions();
app.MapRazorPages();

app.Run();