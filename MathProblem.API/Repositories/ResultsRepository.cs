using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

class ResultsRepository : MemoryRepository<Result>, IRepository<int, Result>
{
}
