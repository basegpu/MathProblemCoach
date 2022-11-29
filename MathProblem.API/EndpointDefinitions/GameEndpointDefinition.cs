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
		app.MapGet(_path, GetNumber).Produces<int>(200);
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<IGameRepository, GameRepository>();
		services.AddSingleton<IRuleProvider, SingleRuleProvider>();
	}

	internal IResult GetNumber(IGameRepository repo)
	{
		return Results.Ok(repo.Running());
	}
}