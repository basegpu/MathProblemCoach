namespace MathProblem.API.Models.Domain;

public record GeneratorConfig(int UpperLimit, double Subtractions = 0.5, bool AllowStep = true, SortedSet<int>? Pillars = null)
{
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(UpperLimit);
        hash.Add(Subtractions);
        hash.Add(AllowStep);
        if (Pillars != null)
        {
            Pillars.ToList().ForEach(p => hash.Add(p));
        }
        return hash.ToHashCode();
    }
}