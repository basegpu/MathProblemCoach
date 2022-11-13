using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

class ProblemRepository : IProblemRepository
{
	private readonly MemoryCache _generators = new("generators");

	public string Add(GeneratorConfig config, int ttl)
	{
		var generator = new ProblemGenerator(config);
		var id = Guid.NewGuid().ToString();
		_generators.Add(id, generator, DateTime.Now.AddSeconds(ttl));
		return id;
	}

	public IDictionary<string, GeneratorConfig> GetAll()
	{
		return _generators.ToDictionary(kvp => kvp.Key, kvp => (kvp.Value as ProblemGenerator)!.Config);
	}

	public bool TryGetConfigById(string id, out GeneratorConfig? config)
	{
		var generator = _generators.Get(id) as ProblemGenerator;
		if (generator == null)
		{
			config = null;
			return false;
		}
		config = generator.Config;
		return true;
	}

	public bool TryGetProblemById(string id, bool next, out Problem? problem)
	{
		var generator = _generators.Get(id) as ProblemGenerator;
		if (generator == null)
		{
			problem = null;
			return false;
		}
		problem = generator.Get(next);
		return true;
	}

	public bool Check(string id, int result)
	{
		var generator = _generators.Get(id) as ProblemGenerator;
		if (generator == null)
		{
			return false;
		}
		return generator.Validate(result);
	}

	public bool TryGetPointsById(string id, out int? points)
	{
		var generator = _generators.Get(id) as ProblemGenerator;
		if (generator == null)
		{
			points = null;
			return false;
		}
		points = generator.Points;
		return true;
	}
}