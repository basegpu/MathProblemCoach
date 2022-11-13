using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        [BindProperty]
        public int? Points { get; set; }

        public SolveModel(ILogger<SolveModel> logger, IProblemRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public void OnGet(Guid id, bool next)
        {
            if (_repo.TryGetProblemById(id.ToString(), next, out var problem) && problem != null)
            {
                Term = problem.Term;
                if (_repo.TryGetPointsById(id.ToString(), out var points))
                {
                    Points = points;
                }
            }
        }

        public IActionResult OnPost()
        {
            var correct = false;
            var id = Request.Path.ToString().Split("/")[2];
            if (Solution != null && _repo.Check(id, Solution.Value))
            {
                correct = true;
            }
            return RedirectToPage("/Solve", new { id = Guid.Parse(id), next = correct });
        }
    }
}