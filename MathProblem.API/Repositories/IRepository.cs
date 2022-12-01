namespace MathProblem.API.Repositories;

public interface IRepository<T>
{
	int Add(T entity);
	bool TryGetById(int id, out T? entity);
	IDictionary<int, T> GetAll();
}