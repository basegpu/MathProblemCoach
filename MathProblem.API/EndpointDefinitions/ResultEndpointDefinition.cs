using Microsoft.AspNetCore.Mvc;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;
using MathProblem.API.SecretSauce;

namespace MathProblem.API.EndpointDefinitions;

public class ResultEndpointDefinition : IEndpointDefinition
{
	private readonly string _path = "/api/results";
	
	public void DefineEndpoints(WebApplication app)
	{
		app.MapPost(_path, Add).Produces<int>(204);
		app.MapGet(_path, GetAll).Produces<IDictionary<int, Result>>(200);
		app.MapGet(_path + "/within", GetWithin).Produces<IDictionary<int, Result>>(200);
	}

	public void DefineServices(IServiceCollection services)
	{
		services.AddSingleton<IRepository<Result>, ResultsRepository>();
	}

	internal IResult Add(IRepository<Result> repo, [FromBody] Result result)
	{
		var id = repo.Add(result);
		return Results.Ok(id);
	}

	internal IResult GetAll(IRepository<Result> repo)
	{
		return Results.Ok(repo.GetAll());
	}

	internal IResult GetWithin(IRepository<Result> repo,
		[FromQuery] DateTime From, [FromQuery] DateTime To)
	{
		var all = repo.GetAll();
		var filtered = all.Where(r => r.Value.TimeSolved < To && r.Value.TimeSolved > From);
		return Results.Ok(filtered);
	}
}