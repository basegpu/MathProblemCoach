using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class ProblemListModel : PageModel
    {
        private readonly IProblemRepository _problems;
        private readonly IRepository<Rules> _rules;

        public IDictionary<int, GeneratorConfig>? Configs { get; set; }

        public ProblemListModel(
            IProblemRepository repo,
            IGameRepository games,
            IRepository<Rules> rules)
        {
            _problems = repo;
            _rules = rules;
        }

        public void OnGet()
        {
            Configs = _problems.GetAll();
        }

        public IActionResult OnPostStart(int problemKey)
        {
            var rulesKey = _rules.GetRulesIdentifier(HttpContext);
            return RedirectToPage("/Start", new { problemKey, rulesKey });
        }
    }
}
