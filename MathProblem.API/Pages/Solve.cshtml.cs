using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StringMath;
using MathProblem.API.Models.Domain;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class SolveModel : PageModel
    {
        private readonly ILogger<SolveModel> _logger;
        private readonly IProblemRepository _repo;

        [BindProperty]
        public string? Term { get; set; }
        
        [BindProperty]
        public int? Solution { get; set; }

        public SolveModel(ILogger<SolveModel> logger, IProblemRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public void OnGet(Guid id)
        {
            if (_repo.TryGetNextById(id.ToString(), out var problem) && problem != null)
            {
                Term = problem.Term;
            }
        }

        public IActionResult OnPost()
        {
            if (Solution != null && Term != null && Term.Eval() == Solution)
            {
                var id = Guid.Parse(Request.Path.ToString().Split("/")[2]);
                return RedirectToPage("/Solve", new { id });
            }
            return RedirectToPage("/Index");
        }
    }
}