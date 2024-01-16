using StringMath;

namespace MathProblem.API.Models.Domain;

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
			if (Operation.IsComplement())
			{
				left = Pyramid.Top(Operation.IsPoint());
			}
			var symbol = Operation.GetChar();
			return $"{left} {symbol} {right}";
		}
	}

	public int Result => (int)Term.Eval();
}