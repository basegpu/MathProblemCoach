namespace MathProblem.API.Models.Domain;

public record Result(
	Problem Problem,
	int Answer,
	bool IsCorrect,
	Guid GameId,
	DateTime TimeSolved);