using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API
{
    public static class Extensions
    {
        public static int GetRulesIdentifier(this IRepository<int, Rules> repo, HttpContext context)
        {
            var rulesKey = context.Session.GetInt32("RulesId");
            if (!rulesKey.HasValue)
            {
                var rules = new Rules(60, 5, 10);
                rulesKey = repo.Add(rules);
                context.Session.SetInt32("RulesId", rulesKey.Value);
            }
            return rulesKey.Value;
        }
    }
}
