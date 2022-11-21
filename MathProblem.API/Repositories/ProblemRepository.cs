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
            new(0, 10, 1, true, null),
            new(7, 9, 0.5, false, new(){2, 3, 4}),
            new(10, 20, 0, false, null),
            new(10, 20, 1, false, null),
            new(11, 12, 0, true, new(){8, 9}),
            new(11, 12, 1, true, new(){8, 9}),
            new(11, 12, 0, true, new(){6, 7}),
            new(11, 12, 1, true, new(){6, 7}),
            new(11, 12, 0.5, true, new(){6, 7, 8, 9}),
            new(13, 13, 0, true, new(){7, 8, 9}),
            new(13, 13, 1, true, new(){7, 8, 9}),
            new(14, 14, 0, true, new(){7, 8, 9}),
            new(14, 14, 1, true, new(){7, 8, 9}),
            new(13, 14, 0.5, true, new(){7, 8, 9}),
            new(11, 14, 0.5, true, new(){6, 7, 8, 9}),
            new(15, 16, 0, true, new(){8, 9}),
            new(15, 16, 1, true, new(){8, 9}),
            new(17, 18, 0, true, new(){9}),
            new(17, 18, 1, true, new(){9}),
            new(15, 18, 0.5, true, new(){8, 9}),
            new(11, 18, 0.5, true, new(){6, 7, 8, 9}),
            new(0, 20, 0.5, true, null)
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
