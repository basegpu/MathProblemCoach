namespace MathProblem.API.Models.View;

public record ProblemConfigPost(bool PointOperation, int LowerLimit, int UpperLimit, double Subtractions, bool AllowSteps, string? Pillars);