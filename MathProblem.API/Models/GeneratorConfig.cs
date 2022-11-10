namespace MathProblem.API.Models;

public record GeneratorConfig(int UpperLimit, double Subtractions = 0.5, List<int>? Pillars = null);