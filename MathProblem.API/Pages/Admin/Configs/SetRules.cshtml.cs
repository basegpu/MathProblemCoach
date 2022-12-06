using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages.Admin.Configs
{
    public class SetRules : PageModel
    {
        private readonly ILogger<SetRules> _logger;
        private readonly IRepository<Rules> _rules;

        [BindProperty]
        public Rules? NewRule { get; set; }

        public SetRules(ILogger<SetRules> logger, IRepository<Rules> repo)
        {
            _logger = logger;
            _rules = repo;
        }

        public void OnGet()
        {
            var rulesKey = _rules.GetRulesIdentifier(HttpContext);
            _rules.TryGetById(rulesKey, out var rule);
            NewRule = rule;
        }

        public IActionResult OnPost()
        {
            if (NewRule != null)
            {
                var id = _rules.Add(NewRule);
                HttpContext.Session.SetInt32("RulesId", id);
                _logger.LogInformation("New rules set: game lasts for {Duration}s, targeting {Target} points, with a penalty of {Penalty} points in case of wrong answer.",
                    NewRule.Duration, NewRule.Target, NewRule.Penalty);
                return RedirectToPage("/index");
            }
            return RedirectToPage(this); 
        }
    }
}
