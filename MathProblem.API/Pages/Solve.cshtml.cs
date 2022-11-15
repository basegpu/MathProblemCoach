using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class SolveModel : PageModel
    {
        private readonly ILogger<SolveModel> _logger;
        private readonly IGameRepository _repo;

        public string? Term { get; private set; }
        public int? Points { get; private set; }
        public int? Achieved { get; private set; }

        [BindProperty]
        public int? Solution { get; set; }

        public SolveModel(ILogger<SolveModel> logger, IGameRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult OnGet(Guid id)
        {
            if (_repo.TryGetGameById(id, out var game) && game!.IsAlive)
            {
                Term = game!.CurrentProblem!.Term;
                Points = game.Points;
                Achieved = (int)(100*(double)Points/game.Rules.Target);
                return Page();
            }
            return RedirectToPage("/feedback", new { id });
        }

        public IActionResult OnPost()
        {
            var id = Guid.Parse(Request.Path.ToString().Split("/")[2]);
            if (_repo.TryGetGameById(id, out var game) && game != null)
            {
                if (game.IsAlive)
                {
                    if (Solution != null)
                    {
                        _logger.LogInformation("Game {Game}: validating {Result} against {Term}.", id, Solution, Term);
                        game.Validate(Solution.Value);
                    }
                    return RedirectToPage("/solve", new { id });
                }
                _logger.LogInformation("Game {Game}: time is over.", id);
                return RedirectToPage("/feedback", new { id });
            }
            _logger.LogError("Game {Game}: something went wrong - back to start.", id);
            return RedirectToPage("/index");
        }
    }
}