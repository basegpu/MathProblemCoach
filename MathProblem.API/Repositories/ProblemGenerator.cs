using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public class ProblemGenerator
{
	public GeneratorConfig Config { get; private set; }
	public int Points { get; private set; } = 0;

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
			_current = MakeProblem();
		}
		return _current;
	}

	public bool Validate(int result)
	{
		if (_current.Result == result)
		{
			Points++;
			return true;
		}
		Points--;
		return false;
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

	private Problem MakeProblem()
	{
		var index = _r.Next(0, _pyramids.Count);
		var pyramid = _pyramids[index];
		var ops = _r.NextDouble() < Config.Subtractions ? Operation.Subtraction : Operation.Addition;
		var alt = _r.Next(0, 2);
		return new Problem(pyramid, ops, Convert.ToBoolean(alt));
	}
}