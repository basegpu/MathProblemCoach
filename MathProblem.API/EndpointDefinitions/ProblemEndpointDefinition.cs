using Microsoft.AspNetCore.Mvc;
using MathProblem.API.Models;
using MathProblem.API.Repositories;
using MathProblem.API.SecretSauce;

namespace MathProblem.API.EndpointDefinitions;

public class ProblemEndpointDefinition : IEndpointDefinition
{
	private readonly string _path = "/api/problems";
	
	public void DefineEndpoints(WebApplication app)
	{
		app.MapPost(_path, CreateProblem).Produces<string>(204);
		app.MapGet(_path + "/{id}", GetConfigById).Produces<GeneratorConfig>(200).Produces(404);
		app.MapGet(_path + "/next/{id}", GetNextProblemById).Produces<Problem>(200).Produces(404);
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<IProblemRepository, ProblemRepository>();
	}

	internal IResult CreateProblem(IProblemRepository repo, [FromBody] GeneratorConfig config, [FromQuery] int ttl)
	{
		var id = repo.Add(config, ttl);
		return Results.Text(id);
	}

	internal IResult GetConfigById(IProblemRepository repo, string id)
	{
		return repo.TryGetConfigById(id, out var config) ? Results.Ok(config) : Results.NotFound();
	}

	internal IResult GetNextProblemById(IProblemRepository repo, string id)
	{
		return repo.TryGetNextById(id, out var problem) ? Results.Ok(problem) : Results.NotFound();
	}
}