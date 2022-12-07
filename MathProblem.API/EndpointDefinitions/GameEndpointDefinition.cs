using Microsoft.AspNetCore.Mvc;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;
using MathProblem.API.SecretSauce;

namespace MathProblem.API.EndpointDefinitions;

public class GameEndpointDefinition : IEndpointDefinition
{
	private readonly string _path = "/api/games";
	
	public void DefineEndpoints(WebApplication app)
	{
		app.MapPost(_path, MakeGame).Produces<Guid>(204);
		app.MapGet(_path, GetGames).Produces<IDictionary<Guid, Game>>(200);
		app.MapGet(_path + "/{id}", GetGameById).Produces<Game>(200).Produces(404);
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<IGameRepository, GameRepository>();
		services.AddSingleton<IConfigRepository, ProblemRepository>();
		services.AddSingleton<IRepository<int, Rules>, RulesRepository>();
	}

	internal IResult MakeGame(
		IGameRepository repo,
		[FromQuery] int configKey,
		[FromQuery] int rulesKey)
	{
		var id = repo.Make(configKey, rulesKey);
		return Results.Ok(id);
	}

	internal IResult GetGames(IGameRepository repo)
	{
		return Results.Ok(repo.GetAll());
	}

	internal IResult GetGameById(IGameRepository repo, Guid id)
	{
		return repo.TryGetById(id, out var game) ? Results.Ok(game) : Results.NotFound();
	}
}