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
		app.MapGet(_path, GetGames).Produces<IDictionary<Guid, Game>>(200);
		app.MapGet(_path + "/{id}", GetGameById).Produces<Game>(200).Produces(404);
		app.MapGet(_path + "/rules", GetRules).Produces<Rules>(200);
		app.MapPost(_path + "/rules", SetRules).Produces<Rules>(204);
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<IGameRepository, GameRepository>();
		services.AddSingleton<IRuleProvider, SingleRuleProvider>();
	}

	internal IResult GetGames(IGameRepository repo)
	{
		return Results.Ok(repo.GetAll());
	}

	internal IResult GetGameById(IGameRepository repo, Guid id)
	{
		return repo.TryGetGameById(id, out var game) ? Results.Ok(game) : Results.NotFound();
	}

	internal IResult GetRules(IRuleProvider repo)
	{
		return Results.Ok(repo.GetCurrent());
	}

	internal IResult SetRules(IRuleProvider repo,
		[FromQuery] int duration = 60,
		[FromQuery] int penalty = 2,
		[FromQuery] int target = 10)
	{
		var newRule = new Rules(duration, penalty, target);
		repo.Set(newRule);
		return Results.Ok(repo.GetCurrent());
	}
}