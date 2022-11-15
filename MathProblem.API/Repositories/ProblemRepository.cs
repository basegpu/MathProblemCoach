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
    private readonly ConcurrentQueue<int> _orderedKeys = new();
    private readonly object _injectLock = new();

    internal ProblemRepository()
    {
        var configs = new List<GeneratorConfig>()
        {
            new(10, 1, true, null),
            new(20, 1, false, null),
            new(20, 0.5, false, null),
            new(20, 0.5, true, new() { 0, 1, 10, 11 })
        };
        configs.ForEach(c => GetOrAdd(c));
    }

    public int GetOrAdd(GeneratorConfig config)
    {
        var hash = config.GetHashCode();
        lock (_injectLock)
        {
            if (!_orderedKeys.Contains(hash))
            {
                _orderedKeys.Enqueue(hash);
                _generators.GetOrAdd(hash, (hash) => new ProblemGenerator(config));
            }
        }
        return hash;
    }

    public IDictionary<int, GeneratorConfig> GetAll()
    {
        lock (_injectLock)
        {
            return _orderedKeys.ToList().ToDictionary(k => k, k => _generators[k].Config);
        }
    }

    public bool TryGetConfigById(int id, out GeneratorConfig? config)
    {
        if (_generators.TryGetValue(id, out var generator))
        {
            config = generator.Config;
            return true;
        }
        config = null;
        return false;   }

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