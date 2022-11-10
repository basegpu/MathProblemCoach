namespace MathProblem.API.Models.View;

public record ProblemConfigPost(int TTL, int UpperLimit, double Subtractions = 0.5, string? Pillars = null);