using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API
{
    public static class Extensions
    {
        private const string _rulesKey = "RulesId";
        private const string _playerKey = "PlayersName";

        public static int GetRulesIdentifier(this HttpContext context, IRepository<int, Rules> repo)
        {
            var rulesKey = context.Session.GetInt32(_rulesKey);
            if (!rulesKey.HasValue)
            {
                var rules = new Rules(60, 5, 10);
                rulesKey = repo.Add(rules);
                context.SetRulesIdentifier(rulesKey.Value);
            }
            return rulesKey.Value;
        }

        public static void SetRulesIdentifier(this HttpContext context, int rulesKey)
            => context.Session.SetInt32(_rulesKey, rulesKey);

        public static string GetPlayersName(this HttpContext context)
        {
            var name = context.Session.GetString(_playerKey);
            if (name == null)
            {
                name = "Ben";
                context.SetPlayersName(name);
            }
            return name;
        }

        public static void SetPlayersName(this HttpContext context, string name)
            => context.Session.SetString(_playerKey, name);
    }
}
