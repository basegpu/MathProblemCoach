using Microsoft.OpenApi.Models;
using TestApi.Models;
using TestApi.SecretSauce;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointDefinitions(typeof(Customer), typeof(OpenApiInfo));

var app = builder.Build();
app.UseEndpointDefinitions();

app.Run();