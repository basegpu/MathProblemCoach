namespace MathProblem.API.Models.Domain;

public record Pyramid(int Left, int Right)
{
	public int Top => Left + Right;
}