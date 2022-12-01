using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IProblemRepository : IRepository<GeneratorConfig>
{
	bool TryGetProblemById(int id, out Problem? problem);
}