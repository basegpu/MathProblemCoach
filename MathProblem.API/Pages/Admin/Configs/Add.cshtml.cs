﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Models.View;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages.Admin.Configs
{
    public class AddModel : PageModel
    {
        private readonly ILogger<AddModel> _logger;
        private readonly IProblemRepository _problems;

        [BindProperty]
        public ProblemConfigPost ConfigRequest { get; set; } = new(10, 0.5, true, null);

        public AddModel(ILogger<AddModel> logger, IProblemRepository repo)
        {
            _logger = logger;
            _problems = repo;
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
                ConfigRequest.UpperLimit,
                ConfigRequest.Subtractions,
                ConfigRequest.AllowSteps,
                pillars);
            var problemId = _problems.GetOrAdd(config);
            _logger.LogInformation("New problem configured: {Config} with key {ProblemKey}.", config.ToString(), problemId);
            return RedirectToPage("/problemlist");
        }
    }
}