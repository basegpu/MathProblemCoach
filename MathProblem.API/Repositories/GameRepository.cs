using System.Runtime.Caching;
using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public class GameRepository : IGameRepository
{
    private readonly IConfigRepository _configs;
    private readonly IRepository<int, Rules> _rules;
    private readonly MemoryCache _games = new("games");

    public GameRepository(IConfigRepository configs, IRepository<int, Rules> rules)
    {
        _configs = configs;
        _rules = rules;
    }

    public Guid Add(Game game)
    {
        var id = Guid.NewGuid();
        var ttl = game.Rules.Duration + 60;
        _games.Add(id.ToString(), game, DateTime.Now.AddSeconds(ttl));
        return id;
    }

    public bool TryGetById(Guid id, out Game? game)
    {
        if (_games.Get(id.ToString()) is Game g)
        {
            game = g;
            return true;
        }
        game = null;
        return false;
    }

    public IDictionary<Guid, Game> GetAll() 
    {
        return _games.ToList().ToDictionary(kvp => Guid.Parse(kvp.Key), kvp => (Game)kvp.Value);
    }

    public Guid Make(int configKey, int rulesKey, string player)
    {
        if (!_rules.TryGetById(rulesKey, out var rules) || rules == null)
        {
            throw new KeyNotFoundException($"No rules found for key {rulesKey}.");
        }
        Problem getProplem()
        {
            if (!_configs.TryGetProblemById(configKey, out var problem) || problem == null)
            {
                throw new KeyNotFoundException($"No generator found for key {configKey}.");
            }
            return problem;
        }
        return Add(new Game(rules, player, getProplem));
    }
}
