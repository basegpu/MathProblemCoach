using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class ProblemListModel : PageModel
    {
        private readonly IConfigRepository _configs;
        private readonly IRepository<int, Rules> _rules;

        public IDictionary<int, GeneratorConfig>? Configs { get; set; }

        public ProblemListModel(
            IConfigRepository configs,
            IGameRepository games,
            IRepository<int, Rules> rules)
        {
            _configs = configs;
            _rules = rules;
        }

        public void OnGet()
        {
            Configs = _configs.GetAll();
        }

        public IActionResult OnPostStart(int configKey)
        {
            var rulesKey = _rules.GetRulesIdentifier(HttpContext);
            return RedirectToPage("/Start", new { configKey, rulesKey });
        }
    }
}
