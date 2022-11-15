namespace MathProblem.API.Models.View;

public record ProblemConfigPost(int LowerLimit, int UpperLimit, double Subtractions, bool AllowSteps, string? Pillars);