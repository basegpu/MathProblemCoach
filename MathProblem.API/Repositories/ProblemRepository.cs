using System.Collections.Concurrent;
using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

class ProblemRepository : IProblemRepository
{
	/// repo for generators: concurrent dictionary, persisting storage, config hash is key
	/// generators themself generate problems only, no game/session info whatsoever
	/// generators can be configured statically or loaded from file
	/// generators can be viewed (listed) and managed (added/edited/deleted) 

	private readonly ConcurrentDictionary<int, ProblemGenerator> _generators = new();

	internal ProblemRepository()
    {
		var config = new GeneratorConfig(20, 0.5, new() { 0, 1, 10, 11 });
		_generators.GetOrAdd(config.GetHashCode(), new ProblemGenerator(config));
    }

	public int GetOrAdd(GeneratorConfig config)
	{
		var hash = config.GetHashCode();
		_generators.GetOrAdd(hash, (hash) => new ProblemGenerator(config));
		return hash;
	}

	public IDictionary<int, GeneratorConfig> GetAll()
	{
		return _generators.ToDictionary(kvp => kvp.Key, kvp => (kvp.Value).Config);
	}

	public bool TryGetConfigById(int id, out GeneratorConfig? config)
	{
		if (_generators.TryGetValue(id, out var generator))
        {
			config = generator.Config;
			return true;
		}
        config = null;
        return false;	}

	public bool TryGetProblemById(int id, out Problem? problem)
	{
		if (_generators.TryGetValue(id, out var generator))
		{
			problem = generator.MakeProblem();
			return true;
		}
        problem = null;
        return false;
	}
}