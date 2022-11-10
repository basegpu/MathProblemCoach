using Microsoft.OpenApi.Models;
using MathProblem.API.SecretSauce;

namespace MathProblem.API.EndpointDefinitions;

public class SwaggerEndpointDefinition : IEndpointDefinition
{
    private readonly string _name = "MathProblem.API";
    private readonly string _version = "v1";

    public void DefineEndpoints(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{_version}/swagger.json", $"{_name} {_version}"));
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