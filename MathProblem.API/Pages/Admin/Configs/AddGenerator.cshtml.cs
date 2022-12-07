using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Models.View;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages.Admin.Configs
{
    public class AddGenerator : PageModel
    {
        private readonly ILogger<AddGenerator> _logger;
        private readonly IConfigRepository _configs;

        [BindProperty]
        public ProblemConfigPost ConfigRequest { get; set; } = new(0, 10, 0.5, true, null);

        public AddGenerator(ILogger<AddGenerator> logger, IConfigRepository configs)
        {
            _logger = logger;
            _configs = configs;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            SortedSet<int>? pillars = null;
            if (ConfigRequest.Pillars != null)
            {
                var pList = ConfigRequest.Pillars.Split(",").Select(p => int.Parse(p));
                pillars = new(pList.OrderBy(p => p));
            }
            var config = new GeneratorConfig(
                ConfigRequest.LowerLimit,
                ConfigRequest.UpperLimit,
                ConfigRequest.Subtractions,
                ConfigRequest.AllowSteps,
                pillars);
            var problemId = _configs.Add(config);
            _logger.LogInformation("New problem configured: {Config} with key {ProblemKey}.", config.ToString(), problemId);
            return RedirectToPage("/problemlist");
        }
    }
}
