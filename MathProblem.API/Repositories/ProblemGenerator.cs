using MathProblem.API.Models;

namespace MathProblem.API.Repositories;

public class ProblemGenerator
{
	public GeneratorConfig Config { get; private set; }

	private readonly Random _r = new Random();
	
	public ProblemGenerator(GeneratorConfig config)
	{
		Config = config;
	}

	public Problem Next()
	{
		var upperRange = Config.UpperLimit + 1;
		var left = _r.Next(0, upperRange);
		var right = _r.Next(0, upperRange);
		var ops = _r.Next(0, 2);
		return new Problem(left, right, (Operation)ops);
	}
}