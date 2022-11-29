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
        private readonly IRuleProvider _rules;

        public IDictionary<int, GeneratorConfig>? Configs { get; set; }

        public ProblemListModel(
            ILogger<ProblemListModel> logger,
            IProblemRepository repo,
            IGameRepository games,
            IRuleProvider rules)
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
            var rules = _rules.GetCurrent();
            var id = _games.Make(problemKey, rules);
            _logger.LogInformation("New session started: {ID}, lasting for {TTL} seconds.", id, rules.Duration);
            return RedirectToPage("/Solve", new { id, success = true });
        }
    }
}
