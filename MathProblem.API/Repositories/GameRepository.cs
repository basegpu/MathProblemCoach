using System.Runtime.Caching;
using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public class GameRepository : IGameRepository
{
    private readonly Func<int, Problem?> _newProblem;
    private readonly MemoryCache _games = new("games");

    public GameRepository(IProblemRepository problems)
    {
        _newProblem = (id) => problems.TryGetProblemById(id, out var problem) ? problem : null;
    }

    public Guid Make(int generatorId, Rules rules)
    {
        Problem getProplem()
        {
            var p = _newProblem(generatorId);
            if (p == null)
            {
                throw new KeyNotFoundException($"No generator found for key {generatorId}.");
            }
            return p;
        }
        var game = new Game(rules, getProplem);
        var id = Guid.NewGuid();
        _games.Add(id.ToString(), game, DateTime.Now.AddSeconds(rules.Duration + 60));
        return id;
    }

    public bool TryGetGameById(Guid id, out Game? game)
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
}
