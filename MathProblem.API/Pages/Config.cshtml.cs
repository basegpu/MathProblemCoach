using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Models.View;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class ConfigModel : PageModel
    {
        private readonly ILogger<ConfigModel> _logger;
        private readonly IProblemRepository _problems;
        private readonly IGameRepository _games;

        [BindProperty]
        public ProblemConfigPost ConfigRequest { get; set; } = new(10, 0.5, true, null);

        public ConfigModel(ILogger<ConfigModel> logger, IProblemRepository repo, IGameRepository games)
        {
            _logger = logger;
            _problems = repo;
            _games = games;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            SortedSet<int>? pillars = null;
            if (ConfigRequest.Pillars != null)
            {
                var pList = ConfigRequest.Pillars.Split(",").Select(p => int.Parse(p));
                pillars = new(pList.OrderBy(p => p));
            }
            var config = new GeneratorConfig(
                ConfigRequest.UpperLimit,
                ConfigRequest.Subtractions,
                ConfigRequest.AllowSteps,
                pillars);
            var problemId = _problems.GetOrAdd(config);
            int ttl = 60;
            var id = _games.Make(problemId, new(ttl, 2, 10));
            _logger.LogInformation("New session started: {ID}, lasting for {TTL} seconds.", id, ttl);
            return RedirectToPage("/Solve", new { id });
        }
    }
}
