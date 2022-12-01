using Microsoft.AspNetCore.Mvc;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;
using MathProblem.API.SecretSauce;

namespace MathProblem.API.EndpointDefinitions;

public class RulesEndpointDefinition : IEndpointDefinition
{
	private readonly string _path = "/api/rules";
	
	public void DefineEndpoints(WebApplication app)
	{
		app.MapPost(_path, CreateRules).Produces<int>(204);
		app.MapGet(_path, GetAll).Produces<IDictionary<int, Rules>>(200);
		app.MapGet(_path + "/current", Current).Produces<Rules>(200);
		app.MapGet(_path + "/{id}", GetRulesById).Produces<Rules>(200).Produces(404);
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<IRuleProvider, SingleRuleProvider>();
	}

	internal IResult CreateRules(IRuleProvider repo,
		[FromQuery] int duration = 60,
		[FromQuery] int penalty = 2,
		[FromQuery] int target = 10)
	{
		var newRule = new Rules(duration, penalty, target);
		var id = repo.Add(newRule);
		return Results.Ok(id);
	}

	internal IResult GetAll(IRuleProvider repo)
	{
		return Results.Ok(repo.GetAll());
	}

	internal IResult Current(IRuleProvider repo)
	{
		return Results.Ok(repo.GetCurrent());
	}

	internal IResult GetRulesById(IRuleProvider repo, int id)
	{
		return repo.TryGetById(id, out var rules) ? Results.Ok(rules) : Results.NotFound();
	}
}