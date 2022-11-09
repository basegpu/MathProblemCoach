namespace MathProblem.API.Models;

public record Pyramid(int Left, int Right)
{
	public int Top => Left + Right;
}