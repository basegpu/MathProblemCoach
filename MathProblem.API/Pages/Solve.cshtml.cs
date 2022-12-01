using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Repositories;
using MathProblem.API.Models.Domain;

namespace MathProblem.API.Pages
{
    public class SolveModel : PageModel
    {
        [BindProperty]
        public int? Solution { get; set; }
        [BindProperty]
        public Guid Id { get; set; }
        [BindProperty]
        public bool Success { get; set; }

        public string? Term { get; private set; }
        public int? Points { get; private set; }
        public int? Achieved { get; private set; }
        public Pyramid? Pyramid { get; private set; }

        private readonly ILogger<SolveModel> _logger;
        private readonly IGameRepository _repo;

        public SolveModel(ILogger<SolveModel> logger, IGameRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult OnGet(Guid id, bool success)
        {
            Id = id;
            Success = success;
            if (_repo.TryGetGameById(Id, out var game) && game!.IsAlive)
            {
                Term = game!.CurrentProblem!.Term;
                Pyramid = game!.CurrentProblem!.Pyramid;
                Points = game.Points;
                Achieved = (int)(100*(double)Points/game.Rules.Target);
                return Page();
            }
            return RedirectToPage("/feedback", new { Id });
        }

        public IActionResult OnPost()
        {
            if (_repo.TryGetGameById(Id, out var game) && game != null)
            {
                if (game.IsAlive)
                {
                    if (Solution != null)
                    {
                        _logger.LogInformation("Game {Game}: validating {Result} against {Term}.", Id, Solution, Term);
                        Success = game.Validate(Solution.Value);
                    }
                    return RedirectToPage("/solve", new { Id, Success });
                }
                _logger.LogInformation("Game {Game}: time is over.", Id);
                return RedirectToPage("/feedback", new { Id });
            }
            _logger.LogError("Game {Game}: something went wrong - back to start.", Id);
            return RedirectToPage("/index");
        }
    }
}