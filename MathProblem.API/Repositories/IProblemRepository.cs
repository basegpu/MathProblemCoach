using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IProblemRepository
{
	int GetOrAdd(GeneratorConfig config);
	bool TryGetConfigById(int id, out GeneratorConfig? config);
	IDictionary<int, GeneratorConfig> GetAll();
	bool TryGetProblemById(int id, out Problem? problem);
}