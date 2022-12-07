using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class StartModel : PageModel
    {
        private readonly ILogger<ProblemListModel> _logger;
        private readonly IProblemRepository _problems;
        private readonly IGameRepository _games;
        private readonly IRepository<Rules> _rules;

        public StartModel(
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

        public IActionResult OnGet(int problemKey, int rulesKey)
        {
            var game = MakeGame(problemKey, rulesKey);
            var gameId = _games.Add(game);
            _logger.LogInformation("New session started: {ID}, lasting for {TTL} seconds.", gameId, game.Rules.Duration);
            return RedirectToPage("/Solve", new { gameId });
        }

        private Game MakeGame(int problemKey, int rulesKey)
        {
            if (!_rules.TryGetById(rulesKey, out var rules) || rules == null)
            {
                throw new KeyNotFoundException($"No rules found for key {rulesKey}.");
            }
            Problem getProplem()
            {
                if (!_problems.TryGetProblemById(problemKey, out var problem) || problem == null)
                {
                    throw new KeyNotFoundException($"No generator found for key {problemKey}.");
                }
                return problem;
            }
            return new Game(rules, getProplem);
        }
    }
}
