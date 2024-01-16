namespace MathProblem.API.Models.Domain;

public record Pyramid(int Left, int Right)
{
	public int Top(bool forPointOperation = false){
		return forPointOperation switch
		{
			false => Left + Right,
			true => Left * Right
		};
	}
}