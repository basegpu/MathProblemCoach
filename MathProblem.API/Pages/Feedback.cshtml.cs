﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MathProblem.API.Repositories;

namespace MathProblem.API.Pages
{
    public class FeedbackModel : PageModel
    {
        private readonly ILogger<SolveModel> _logger;
        private readonly IGameRepository _repo;

        [BindProperty]
        public int? Target { get; set; }

        [BindProperty]
        public int? Points { get; set; }

        [BindProperty]
        public string? Message { get; set; }

        public FeedbackModel(ILogger<SolveModel> logger, IGameRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult OnGet(Guid id)
        {
            if (_repo.TryGetGameById(id, out var game) && game != null)
            {
                Points = game.Points;
                Target = game.Rules.Target;
                Message = MakeMessage((double)Points / (double)Target);
                return Page();
            }
            return RedirectToPage("/Index");
        }

        private static string MakeMessage(double success)
        {
            if (success == 0)
                return "Da ist was völlig schief gelaufen...";
            else if (success < 0.2)
                return "Das geht doch besser - am besten gleich nochmals.";
            else if (success < 0.4)
                return "Scheint langsam zu klappen - bleib dran.";
            else if (success < 0.6)
                return "Nicht schlecht, aber ein bischen Training brauchts noch.";
            else if (success < 0.8)
                return "Jetzt bist du deinem Ziel schon ganz nahe.";
            else if (success == 1.0)
                return "Super, du hast das Ziel erreicht!";
            else if (success < 1.2)
                return "Wow, jetzt gibst du aber Gas...";
            else if (success < 1.5)
                return "Ok, diese Rechnungen hast du im Griff :).";
            else
                return "Jetzt übertreibst du aber.";
        }
    }
}