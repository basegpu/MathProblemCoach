namespace MathProblem.API.Models;

public record Problem(Pyramid Pyramid, Operation Operation, bool Alternative)
{
	public string Term
	{
		get
		{
			var left = Pyramid.Left;
			var right = Pyramid.Right;
			if (Alternative)
			{
				(left, right) = (right, left);
			}
			if (Operation == Operation.Subtraction)
			{
				left = Pyramid.Top;
			}
			var symbol = OperationChars.Get(Operation);
			return $"{left}{symbol}{right}";
		}
	}
}

