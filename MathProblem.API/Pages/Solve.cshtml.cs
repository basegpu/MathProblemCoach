using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class SolveModel : PageModel
    {
        private readonly ILogger<SolveModel> _logger;
        private readonly IGameRepository _repo;

        [BindProperty]
        public string? Term { get; set; }
        
        [BindProperty]
        public int? Solution { get; set; }

        [BindProperty]
        public int? Points { get; set; }

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
                return Page();
            }
            return RedirectToPage("/Feedback", new { id });
        }

        public IActionResult OnPost()
        {
            var id = Guid.Parse(Request.Path.ToString().Split("/")[2]);
            if (_repo.TryGetGameById(id, out var game) && game != null)
            {
                if (game.IsAlive)
                {
                    var correct = Solution != null && game.Validate(Solution.Value);
                    _logger.LogInformation("Game {Game}: entered result {Result} is {Validation}.", id, Solution, correct);
                    return RedirectToPage("/Solve", new { id, next = correct });
                }
                _logger.LogInformation("Game {Game}: time is over.", id);
                return RedirectToPage("/Feedback", new { id });
            }
            _logger.LogError("Game {Game}: something went wrong - back to start.", id);
            return RedirectToPage("/Index");
        }
    }
}