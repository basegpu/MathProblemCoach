using Microsoft.AspNetCore.Mvc;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;
using MathProblem.API.SecretSauce;

namespace MathProblem.API.EndpointDefinitions;

public class ProblemEndpointDefinition : IEndpointDefinition
{
	private readonly string _path = "/api/problems";
	
	public void DefineEndpoints(WebApplication app)
	{
		app.MapPost(_path, CreateProblem).Produces<int>(204);
		app.MapGet(_path, GetAll).Produces<IDictionary<int, GeneratorConfig>>(200);
		app.MapGet(_path + "/{id}", GetConfigById).Produces<GeneratorConfig>(200).Produces(404);
		app.MapGet(_path + "/next/{id}", GetNextProblemById).Produces<Problem>(200).Produces(404);
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<IConfigRepository, ProblemRepository>();
	}

	internal IResult CreateProblem(IConfigRepository repo, [FromBody] GeneratorConfig config)
	{
		var id = repo.Add(config);
		return Results.Ok(id);
	}

	internal IResult GetAll(IConfigRepository repo)
	{
		return Results.Ok(repo.GetAll());
	}

	internal IResult GetConfigById(IConfigRepository repo, int id)
	{
		return repo.TryGetById(id, out var config) ? Results.Ok(config) : Results.NotFound();
	}

	internal IResult GetNextProblemById(IConfigRepository repo, int id)
	{
		return repo.TryGetProblemById(id, out var problem) ? Results.Ok(problem) : Results.NotFound();
	}
}