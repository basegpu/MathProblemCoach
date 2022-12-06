namespace MathProblem.API.Models.Domain;

public record Result(
	string Term,
	int Answer,
	bool IsCorrect,
	Guid GameId,
	DateTime TimeSolved);