namespace MathProblem.API.Models.View;

public record ProblemConfigPost(int TTL, int UpperLimit, double Subtractions, string? Pillars);