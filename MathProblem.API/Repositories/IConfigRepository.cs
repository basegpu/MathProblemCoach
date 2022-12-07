using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IConfigRepository : IRepository<int, GeneratorConfig>
{
	bool TryGetProblemById(int id, out Problem? problem);
}