using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IGameRepository
{
	Guid Add(Game game);
    bool TryGetGameById(Guid id, out Game? game);
    IDictionary<Guid, Game> GetAll();
}