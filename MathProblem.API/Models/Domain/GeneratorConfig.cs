namespace MathProblem.API.Models.Domain;

public record GeneratorConfig(
    int LowerLimit,
    int UpperLimit,
    double Subtractions = 0.5,
    bool AllowStep = true,
    SortedSet<int>? Pillars = null)
{
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(LowerLimit);
        hash.Add(UpperLimit);
        hash.Add(Subtractions);
        hash.Add(AllowStep);
        if (Pillars != null)
        {
            Pillars.ToList().ForEach(p => hash.Add(p));
        }
        return hash.ToHashCode();
    }

    public override string ToString()
    {
        return $"Lim{LowerLimit}:{UpperLimit}-Sub{Subtractions:0.##}-{(AllowStep ? "steps-" : "")}Pil[{(Pillars != null ? string.Join(",", Pillars) : "")}]";
    }

    public string Description
    {
        get
        {
            var ops = "Plus/Minus";
            var magn = Math.Abs(Subtractions - 0.5);
            if (magn >= 0.25)
            {
                ops = "vor allem";
                if (magn >= 0.5)
                {
                    ops = "nur";
                }
                ops += $" {(Subtractions > 0.5 ? "Minus" : "Plus")}";
            }
            var desc = $"von {LowerLimit} bis {UpperLimit}, {ops}";
            if (UpperLimit > 10)
            {
                var step = $"{(AllowStep ? "mit" : "ohne")} Zehnerschritt";
                desc += $", {step}";
            }
            if (Pillars != null && Pillars.Any())
            {
                var pil = $"ein Summand aus ({string.Join(", ", Pillars)})";
                desc += $", {pil}";
            }
            return desc;
        }
    }
}