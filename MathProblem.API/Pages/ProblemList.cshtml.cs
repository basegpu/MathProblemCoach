using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class ProblemListModel : PageModel
    {
        private readonly ILogger<ProblemListModel> _logger;
        private readonly IProblemRepository _problems;
        private readonly IGameRepository _games;
        private readonly IRepository<Rules> _rules;

        public IDictionary<int, GeneratorConfig>? Configs { get; set; }

        public ProblemListModel(
            ILogger<ProblemListModel> logger,
            IProblemRepository repo,
            IGameRepository games,
            IRepository<Rules> rules)
        {
            _logger = logger;
            _problems = repo;
            _games = games;
            _rules = rules;
        }

        public void OnGet()
        {
            Configs = _problems.GetAll();
        }

        public IActionResult OnPostStart(int problemKey)
        {
            var rulesKey = _rules.GetRulesIdentifier(HttpContext);
            var game = MakeGame(problemKey, rulesKey);
            var gameId = _games.Add(game);
            _logger.LogInformation("New session started: {ID}, lasting for {TTL} seconds.", gameId, game.Rules.Duration);
            return RedirectToPage("/Solve", new { gameId, success = true });
        }

        private Game MakeGame(int problemKey, int rulesKey)
        {
            Problem getProplem()
            {
                if (!_problems.TryGetProblemById(problemKey, out var problem) || problem == null)
                {
                    throw new KeyNotFoundException($"No generator found for key {problemKey}.");
                }
                return problem;
            }
            if (!_rules.TryGetById(rulesKey, out var rules) || rules == null)
            {
                throw new KeyNotFoundException($"No rules found for key {rulesKey}.");
            }
            return new Game(rules, getProplem);
        }
    }
}
