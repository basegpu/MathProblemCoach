using MathProblem.API.Models.Domain;

namespace MathProblem.API.Repositories;

class ProblemRepository : MemoryRepository<GeneratorConfig, ProblemGenerator>, IProblemRepository
{
    public ProblemRepository() : base(
        (config) => new ProblemGenerator(config),
        (generator) => generator.Config)
    {
        var configs = new List<GeneratorConfig>()
        {
            new(0, 10, 1, true, null),
            new(10, 20, 0, false, null),
            new(10, 20, 1, false, null),
            new(11, 12, 0, true, new(){8, 9}),
            new(11, 12, 1, true, new(){8, 9}),
            new(11, 12, 0, true, new(){6, 7}),
            new(11, 12, 1, true, new(){6, 7}),
            new(11, 12, 0, true, new(){6, 7, 8, 9}),
            new(11, 12, 1, true, new(){6, 7, 8, 9}),
            new(11, 12, 0.5, true, new(){6, 7, 8, 9}),
            new(13, 13, 0, true, new(){7, 8, 9}),
            new(13, 13, 1, true, new(){7, 8, 9}),
            new(14, 14, 0, true, new(){7, 8, 9}),
            new(14, 14, 1, true, new(){7, 8, 9}),
            new(13, 14, 0, true, new(){7, 8, 9}),
            new(13, 14, 1, true, new(){7, 8, 9}),
            new(13, 14, 0.5, true, new(){7, 8, 9}),
            new(11, 14, 0, true, new(){6, 7, 8, 9}),
            new(11, 14, 1, true, new(){6, 7, 8, 9}),
            new(11, 14, 0.5, true, new(){6, 7, 8, 9}),
            new(15, 16, 0, true, new(){8, 9}),
            new(15, 16, 1, true, new(){8, 9}),
            new(17, 18, 0, true, new(){9}),
            new(17, 18, 1, true, new(){9}),
            new(15, 18, 0, true, new(){8, 9}),
            new(15, 18, 1, true, new(){8, 9}),
            new(15, 18, 0.5, true, new(){8, 9}),
            new(11, 18, 0, true, new(){6, 7, 8, 9}),
            new(11, 18, 1, true, new(){6, 7, 8, 9}),
            new(11, 18, 0.5, true, new(){6, 7, 8, 9}),
            new(0, 20, 0, true, null),
            new(0, 20, 1, true, null),
            new(0, 20, 0.5, true, null)
        };
        configs.ForEach(c => Add(c));
    }

    public bool TryGetProblemById(int id, out Problem? problem)
    {
        if (_repo.TryGetValue(id, out var generator) && generator != null)
        {
            problem = generator.MakeProblem();
            return true;
        }
        problem = null;
        return false;
    }
}
