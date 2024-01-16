namespace MathProblem.API.Models.Domain;

public record GeneratorConfig(
    bool PointOperation,
    int LowerLimit,
    int UpperLimit,
    double Complement = 0.5,
    bool AllowStep = true,
    SortedSet<int>? Pillars = null)
{
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(PointOperation);
        hash.Add(LowerLimit);
        hash.Add(UpperLimit);
        hash.Add(Complement);
        hash.Add(AllowStep);
        if (Pillars != null)
        {
            Pillars.ToList().ForEach(p => hash.Add(p));
        }
        return hash.ToHashCode();
    }

    public override string ToString()
    {
        return $"{(PointOperation ? "Point" : "Line")}-Lim{LowerLimit}:{UpperLimit}-Cmpl{Complement:0.##}-{(AllowStep ? "steps-" : "")}Pil[{(Pillars != null ? string.Join(",", Pillars) : "")}]";
    }

    public string Description
    {
        get
        {
            var op1 = OperationExtensions.Make(false, PointOperation).GetDescription();
            var op2 = OperationExtensions.Make(true, PointOperation).GetDescription();
            var ops = $"{op1}/{op2}";
            var magn = Math.Abs(Complement - 0.5);
            if (magn >= 0.25)
            {
                ops = "vor allem";
                if (magn >= 0.5)
                {
                    ops = "nur";
                }
                ops += $" {(Complement > 0.5 ? op2 : op1)}";
            }
            var desc = $"von {LowerLimit} bis {UpperLimit}, {ops}";
            if (UpperLimit > 10 && !PointOperation)
            {
                var step = $"{(AllowStep ? "mit" : "ohne")} Zehnerschritt";
                desc += $", {step}";
            }
            if (Pillars != null && Pillars.Any())
            {
                var pil = $"ein {(PointOperation ? "Faktor" : "Summand")} aus ({string.Join(", ", Pillars)})";
                desc += $", {pil}";
            }
            return desc;
        }
    }
}