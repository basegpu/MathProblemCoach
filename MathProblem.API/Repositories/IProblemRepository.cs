using System.Collections.Generic;
using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IProblemRepository
{
	string Add(GeneratorConfig config, int ttl);
	bool TryGetConfigById(string id, out GeneratorConfig? config);
	IDictionary<string, GeneratorConfig> GetAll();
	bool TryGetProblemById(string id, bool next, out Problem? problem);
}