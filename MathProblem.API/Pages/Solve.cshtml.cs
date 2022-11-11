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
        private Guid? _id;

        [BindProperty]
        public Problem? Problem { get; set; }
        
        [BindProperty]
        public int? Solution { get; set; }

        public SolveModel(ILogger<SolveModel> logger, IProblemRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public void OnGet(Guid id)
        {
            _id = id;
            if (_repo.TryGetNextById(_id.ToString(), out var p))
            {
                Problem = p;
            }
        }

        public IActionResult OnPost()
        {
            if (Problem != null && Solution != null && Problem.Term.Eval() == Solution)
            {
                return RedirectToPage("/Solve", new { id = _id });
            }
            return RedirectToPage("/Index");
        }
    }
}