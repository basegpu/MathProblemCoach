using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages.Admin.Configs
{
    public class SetRules : PageModel
    {
        private readonly ILogger<SetRules> _logger;
        private readonly IRuleProvider _rules;

        [BindProperty]
        public Rules NewRule { get; set; }

        public SetRules(ILogger<SetRules> logger, IRuleProvider repo)
        {
            _logger = logger;
            _rules = repo;
            NewRule = _rules.GetCurrent();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _rules.Add(NewRule);
            _logger.LogInformation("New rules set: game lasts for {Duration}s, targeting {Target} points, with a penalty of {Penalty} points in case of wrong answer.",
                NewRule.Duration, NewRule.Target, NewRule.Penalty);
            return RedirectToPage("/index");
        }
    }
}
