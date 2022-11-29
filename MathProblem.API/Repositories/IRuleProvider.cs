using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IRuleProvider
{
	void Set(Rules Rule);
    Rules GetCurrent();
}