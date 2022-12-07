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
        [BindProperty(SupportsGet = true)]
        public Guid GameId { get; set; }
        [BindProperty]
        public bool Success { get; set; }

        public string? Term { get; private set; }
        public int? Points { get; private set; }
        public int? Achieved { get; private set; }
        public Pyramid? Pyramid { get; private set; }

        private readonly ILogger<SolveModel> _logger;
        private readonly IGameRepository _repo;
        private readonly IRepository<Result> _results;

        public SolveModel(
            ILogger<SolveModel> logger,
            IGameRepository repo,
            IRepository<Result> results)
        {
            _logger = logger;
            _repo = repo;
            _results = results;
        }

        public IActionResult OnGet(Guid gameId, bool? success)
        {
            Success = success ?? true;
            if (_repo.TryGetGameById(GameId, out var game) && game!.IsAlive)
            {
                Term = game!.CurrentProblem!.Term;
                Pyramid = game!.CurrentProblem!.Pyramid;
                Points = game.Points;
                Achieved = (int)(100*(double)Points/game.Rules.Target);
                return Page();
            }
            return RedirectToPage("/feedback", new { GameId });
        }

        public IActionResult OnPost()
        {
            if (_repo.TryGetGameById(GameId, out var game) && game != null)
            {
                if (game.IsAlive)
                {
                    if (Solution != null)
                    {
                        _logger.LogInformation("Game {Game}: validating {Result} against {Term}.", GameId, Solution, Term);
                        var term = game.CurrentProblem!.Term;
                        Success = game.Validate(Solution.Value);
                        var result = new Result(term, Solution.Value, Success, GameId, DateTime.UtcNow);
                        _results.Add(result);
                    }
                    
                    return RedirectToPage("/solve", new { GameId, Success });
                }
                _logger.LogInformation("Game {Game}: time is over.", GameId);
                return RedirectToPage("/feedback", new { GameId });
            }
            _logger.LogError("Game {Game}: something went wrong - back to start.", GameId);
            return RedirectToPage("/index");
        }
    }
}