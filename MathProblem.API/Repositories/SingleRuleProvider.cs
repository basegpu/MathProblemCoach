using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

public class SingleRuleProvider : IRuleProvider
{
    private Rules _current;

    public SingleRuleProvider()
    {
        _current = new(60, 2, 10);
    }

    public void Set(Rules rule)
    {
        _current = rule;
    }

    public Rules GetCurrent() => _current;
}
