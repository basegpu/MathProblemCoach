using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public class ProblemGenerator
{
	public GeneratorConfig Config { get; private set; }

	private readonly Random _r = new Random();
	private readonly List<Pyramid> _pyramids = new();
	private Problem _current;
	
	public ProblemGenerator(GeneratorConfig config)
	{
		Config = config;
		InitPyramids();
		_current = Get(true);
	}

	public Problem Get(bool next)
	{
		if (next)
		{
			var index = _r.Next(0, _pyramids.Count);
			var pyramid = _pyramids[index];
			var ops = _r.NextDouble() < Config.Subtractions ? Operation.Subtraction : Operation.Addition;
			var alt = _r.Next(0, 2);
			_current = new Problem(pyramid, ops, Convert.ToBoolean(alt));
		}
		return _current;
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
}