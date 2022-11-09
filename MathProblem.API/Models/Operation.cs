namespace MathProblem.API.Models;

public enum Operation : sbyte
{
	Addition,
	Substraction
}

internal static class OperationChars
{
	public static char Get(Operation op) => op switch
	{
		Operation.Addition => '+',
		Operation.Substraction => '-',
		_ => throw new ArgumentOutOfRangeException(nameof(op), $"Not expected operation: {op}.")
	};
}