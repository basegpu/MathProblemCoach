namespace MathProblem.API.Models;

public record GeneratorConfig(int UpperLimit, double Substractions = 0.5, List<int>? Pillars = null);