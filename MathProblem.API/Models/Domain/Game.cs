namespace MathProblem.API.Models.Domain;

public class Game
{
	/// a game has all information like
	/// - function to generate a new problem
	/// - the rules about the game (time, target, etc)
	/// - the current points, current problem 
	/// - the players name
	
	public string Player { get; private set; }
	public int Points { get; private set; }
	public Problem? CurrentProblem { get; private set; }
	public Rules Rules { get; private set; }
	public bool IsAlive => DateTime.Now < _endOfLife;

	private readonly Action _problemMaker;
	private DateTime _endOfLife;

    public Game(Rules rules, string player, Func<Problem> next)
	{
		Rules = rules;
		Player = player;
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