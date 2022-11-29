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
		app.MapPost(_path, CreateProblem).Produces<string>(204);
		app.MapGet(_path, GetAll).Produces<IDictionary<string, GeneratorConfig>>(200);
		app.MapGet(_path + "/{id}", GetConfigById).Produces<GeneratorConfig>(200).Produces(404);
		app.MapGet(_path + "/next/{id}", GetNextProblemById).Produces<Problem>(200).Produces(404);
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<IProblemRepository, ProblemRepository>();
	}

	internal IResult CreateProblem(IProblemRepository repo, [FromBody] GeneratorConfig config)
	{
		var id = repo.GetOrAdd(config);
		return Results.Ok(id);
	}

	internal IResult GetAll(IProblemRepository repo)
	{
		return Results.Ok(repo.GetAll());
	}

	internal IResult GetConfigById(IProblemRepository repo, int id)
	{
		return repo.TryGetConfigById(id, out var config) ? Results.Ok(config) : Results.NotFound();
	}

	internal IResult GetNextProblemById(IProblemRepository repo, int id)
	{
		return repo.TryGetProblemById(id, out var problem) ? Results.Ok(problem) : Results.NotFound();
	}
}