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

        public IDictionary<int, GeneratorConfig>? Configs { get; set; }

        public ProblemListModel(ILogger<ProblemListModel> logger, IProblemRepository repo, IGameRepository games)
        {
            _logger = logger;
            _problems = repo;
            _games = games;
        }

        public void OnGet()
        {
            Configs = _problems.GetAll();
        }

        public IActionResult OnPostStart(int problemKey)
        {
            int ttl = 60;
            var id = _games.Make(problemKey, new(ttl, 2, 10));
            _logger.LogInformation("New session started: {ID}, lasting for {TTL} seconds.", id, ttl);
            return RedirectToPage("/Solve", new { id });
        }
    }
}
