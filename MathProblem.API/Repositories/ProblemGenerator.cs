using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public class ProblemGenerator
{
	public GeneratorConfig Config { get; private set; }

	private readonly Random _r = new();
	private readonly List<Pyramid> _pyramids = new();
	
	public ProblemGenerator(GeneratorConfig config)
	{
		Config = config;
		InitPyramids();
	}

	private void InitPyramids()
	{
		// all possible pyramids
		for (var left = 0; left <= Config.UpperLimit; left++)
		{
			for (var right = left; right <= Config.UpperLimit; right++)
			{
				if (left + right <= Config.UpperLimit)
				{
					_pyramids.Add(new(left, right));
				}
			}
		}
		// filter out not matching pillars
		if (Config.Pillars != null)
		{
			var pillars = Config.Pillars;
			_pyramids.RemoveAll(p => !pillars.Contains(p.Left) && !pillars.Contains(p.Right));
		}
	}

	public Problem MakeProblem()
	{
		var index = _r.Next(0, _pyramids.Count);
		var pyramid = _pyramids[index];
		var ops = _r.NextDouble() < Config.Subtractions ? Operation.Subtraction : Operation.Addition;
		var alt = _r.Next(0, 2);
		return new Problem(pyramid, ops, Convert.ToBoolean(alt));
	}
}