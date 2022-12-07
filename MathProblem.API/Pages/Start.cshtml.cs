using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class StartModel : PageModel
    {
        private readonly ILogger<StartModel> _logger;
        private readonly IGameRepository _games;

        public StartModel(
            ILogger<StartModel> logger,
            IGameRepository games)
        {
            _logger = logger;
            _games = games;
        }

        public IActionResult OnGet(int configKey, int rulesKey)
        {
            var gameId = _games.Make(configKey, rulesKey);
            if (_games.TryGetById(gameId, out var game))
            {
                _logger.LogInformation("New session started: {ID}, lasting for {TTL} seconds.", gameId, game!.Rules.Duration);
                return RedirectToPage("/Solve", new { gameId });
            }
            return RedirectToPage("/Index");
        }
    }
}
