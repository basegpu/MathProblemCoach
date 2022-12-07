using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IGameRepository : IRepository<Guid, Game>
{
    Guid Make(int configKey, int rulesKey);
}