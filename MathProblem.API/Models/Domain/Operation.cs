namespace MathProblem.API.Models.Domain
{
	public enum Operation : sbyte
	{
		Addition,
		Subtraction,
		Multiplication,
		Division
	}

	public static class OperationExtensions
	{
		public static char GetChar(this Operation op)
		{
			return op switch
			{
				Operation.Addition => '+',
				Operation.Subtraction => '-',
				Operation.Multiplication => '*',
				Operation.Division => ':',
				_ => throw new ArgumentOutOfRangeException(nameof(op), $"Not expected operation: {op}.")
			};
		}

		public static string GetDescription(this Operation op)
		{
			return op switch
			{
				Operation.Addition => "Plus",
				Operation.Subtraction => "Minus",
				Operation.Multiplication => "Mal",
				Operation.Division => "Geteilt",
				_ => throw new ArgumentOutOfRangeException(nameof(op), $"Not expected operation: {op}.")
			};
		}

		public static Operation GetOpposite(this Operation op)
		{
			return op switch
			{
				Operation.Addition => Operation.Subtraction,
				Operation.Subtraction => Operation.Addition,
				Operation.Multiplication => Operation.Division,
				Operation.Division => Operation.Multiplication,
				_ => throw new ArgumentOutOfRangeException(nameof(op), $"Not expected operation: {op}.")
			};
		}

		public static bool IsComplement(this Operation op)
		{
			return op switch
			{
				Operation.Addition => false,
				Operation.Subtraction => true,
				Operation.Multiplication => false,
				Operation.Division => true,
				_ => throw new ArgumentOutOfRangeException(nameof(op), $"Not expected operation: {op}.")
			};
		}

		public static bool IsPoint(this Operation op)
		{
			return op switch
			{
				Operation.Addition => false,
				Operation.Subtraction => false,
				Operation.Multiplication => true,
				Operation.Division => true,
				_ => throw new ArgumentOutOfRangeException(nameof(op), $"Not expected operation: {op}.")
			};
		}

		public static Operation Make(bool isComplement, bool isPoint)
		{
			return (isComplement, isPoint) switch
			{
				(false, false) => Operation.Addition,
				(true, false) => Operation.Subtraction,
				(false, true) => Operation.Multiplication,
				(true, true) => Operation.Division
			};
		}
	}
}