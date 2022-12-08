using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages.Admin.Configs
{
    public class SetNameModel : PageModel
    {
        private readonly ILogger<SetNameModel> _logger;

        [BindProperty]
        public string? Name { get; set; }

        public SetNameModel(ILogger<SetNameModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (Name != null)
            {
                HttpContext.SetPlayersName(Name);
                _logger.LogInformation("New players name set: {Name}", Name);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
