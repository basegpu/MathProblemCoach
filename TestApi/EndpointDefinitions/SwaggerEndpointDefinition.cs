using Microsoft.OpenApi.Models;
using TestApi.SecretSauce;

namespace TestApi.EndpointDefinitions;

public class SwaggerEndpointDefinition : IEndpointDefinition
{
    private readonly string _name = "TestApi";
    private readonly string _version = "v1";

    public void DefineEndpoints(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{_name} {_version}"));
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = _name, Version = _version });
        });
    }
}