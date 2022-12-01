using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public interface IRuleProvider : IRepository<Rules>
{
    Rules GetCurrent();
}