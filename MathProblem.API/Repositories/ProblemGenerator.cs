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
				var pyramid = new Pyramid(left, right);
				if (pyramid.Top() <= Config.UpperLimit && pyramid.Top() >= Config.LowerLimit || Config.PointOperation)
				{
					_pyramids.Add(pyramid);
				}
			}
		}
		// filter out steps over ten
		if (!Config.AllowStep)
        {
			_pyramids.RemoveAll(p => (p.Top() / 10) > Math.Max(p.Left / 10, p.Right / 10));
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
		var ops = OperationExtensions.Make(_r.NextDouble() < Config.Complement, Config.PointOperation);
		var alt = _r.Next(0, 2);
		return new Problem(pyramid, ops, Convert.ToBoolean(alt));
	}
}