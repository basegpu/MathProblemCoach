using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Models.Domain;
using MathProblem.API.Models.View;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class ConfigModel : PageModel
    {
        private readonly ILogger<ConfigModel> _logger;
        private readonly IProblemRepository _repo;

        [BindProperty]
        public ProblemConfigPost ConfigRequest { get; set; } = new(60, 10);

        public ConfigModel(ILogger<ConfigModel> logger, IProblemRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            List<int>? pillars = null;
            if (ConfigRequest.Pillars != null)
            {
                pillars = ConfigRequest.Pillars.Split(",").Select(p => int.Parse(p)).ToList();
            }
            var config = new GeneratorConfig(
                ConfigRequest.UpperLimit,
                ConfigRequest.Subtractions,
                pillars);
            var ttl = ConfigRequest.TTL;
            var id = _repo.Add(config, ttl);
            _logger.LogInformation($"New session started: {id}, lasting for {ttl} seconds.");
        }
    }
}