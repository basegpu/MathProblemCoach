using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

class SingleRuleProvider : MemoryRepository<Rules>, IRuleProvider
{
    public SingleRuleProvider()
    {
        Add(new(60, 2, 10));
    }

    public Rules GetCurrent()
    {
        var id = _orderedKeys.ToArray().Last();
        return _repo[id];
    }
}
