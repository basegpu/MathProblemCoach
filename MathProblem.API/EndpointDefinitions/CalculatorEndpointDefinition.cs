using Microsoft.AspNetCore.Mvc;
using MathProblem.API.SecretSauce;
using StringMath;

namespace MathProblem.API.EndpointDefinitions;

public class CalculatorEndpointDefinition : IEndpointDefinition
{
	private readonly string _path = "/api/calculator";
	
	public void DefineEndpoints(WebApplication app)
	{
		app.MapGet(_path + "/{term}", GetResult);
	}

	public void DefineServices(IServiceCollection services)
	{
	}

	internal string GetResult(string term)
	{
		return term.Eval().ToString();
	}
}