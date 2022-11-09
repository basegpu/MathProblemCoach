using System.Runtime.Caching;
using MathProblem.API.Models;

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

	public bool TryGetNextById(string id, out Problem? problem)
	{
		var generator = _generators.Get(id) as ProblemGenerator;
		if (generator == null)
		{
			problem = null;
			return false;
		}
		problem = generator.Next();
		return true;
	}
}