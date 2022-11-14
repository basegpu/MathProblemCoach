using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IGameRepository
{
	Guid Make(int generatorId, Rules rules);
    bool TryGetGameById(Guid id, out Game? game);
}