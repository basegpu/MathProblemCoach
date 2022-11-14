namespace MathProblem.API.Models.View;

public record ProblemConfigPost(int UpperLimit, double Subtractions, bool AllowSteps, string? Pillars);