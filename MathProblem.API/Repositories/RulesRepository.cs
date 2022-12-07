using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

class RulesRepository : MemoryRepository<Rules>, IRepository<int, Rules>
{
}
