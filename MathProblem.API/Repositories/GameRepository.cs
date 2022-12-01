using System.Runtime.Caching;
using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public class GameRepository : IGameRepository
{
    private readonly MemoryCache _games = new("games");

    public Guid Add(Game game)
    {
        var id = Guid.NewGuid();
        var ttl = game.Rules.Duration + 60;
        _games.Add(id.ToString(), game, DateTime.Now.AddSeconds(ttl));
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
