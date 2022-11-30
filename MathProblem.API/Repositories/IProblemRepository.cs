using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IProblemRepository : IRepository<GeneratorConfig>
{
	bool TryGetProblemById(int id, out Problem? problem);
}

public interface IRepository<T>
{
	int Add(T entity);
	bool TryGetById(int id, out T? entity);
	IDictionary<int, T> GetAll();
}