using MathProblem.API.Models;

namespace MathProblem.API.Repositories;

interface IProblemRepository
{
	string Add(GeneratorConfig config, int ttl);
	bool TryGetConfigById(string id, out GeneratorConfig? config);
	bool TryGetNextById(string id, out Problem? problem);
}