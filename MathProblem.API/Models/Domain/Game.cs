namespace MathProblem.API.Models.Domain;

public class Game
{
	/// a game has all information like
	/// - key to access the generator
	/// - the rules about the game (time, target, etc)
	/// - the current points, current problem <summary>
	/// a game has all information like
	/// </summary>
	public int Points { get; private set; }
	public Problem? CurrentProblem { get; private set; }
	public Rules Rules { get; private set; }
	public bool IsAlive => DateTime.Now < _endOfLife;

	private readonly Action _problemMaker;
	private DateTime _endOfLife;

    public Game(Rules rules, Func<Problem> next)
	{
		Rules = rules;
		_problemMaker = () =>
		{
			CurrentProblem = next();
		};
		Start();
	}

	public bool Validate(int result)
	{
		if (CurrentProblem?.Result == result)
		{
			Points++;
			_problemMaker();
			return true;
		}
		Points = Math.Max(0, Points - Rules.Penalty);
		return false;
	}

	private void Start()
	{
		_problemMaker();
		Points = 0;
		_endOfLife = DateTime.Now.AddSeconds(Rules.Duration);
	}
}