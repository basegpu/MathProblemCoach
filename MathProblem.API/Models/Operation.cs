namespace MathProblem.API.Models;

public enum Operation : sbyte
{
	Addition,
	Subtraction
}

internal static class OperationChars
{
	public static char Get(Operation op) => op switch
	{
		Operation.Addition => '+',
		Operation.Subtraction => '-',
		_ => throw new ArgumentOutOfRangeException(nameof(op), $"Not expected operation: {op}.")
	};
}