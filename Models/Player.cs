namespace BlackjackGame.Models;

/// <summary>
/// Represents a player, 
/// </summary>
public class Player
{
    public const float STARTING_MONEY_EASY = 30.0f;
    public const float STARTING_MONEY_NORMAL = 30.0f;
    public const float STARTING_MONEY_HARD = 30.0f;
    
    /// <summary>The player's current hand this round</summary>
    public Hand _hand;

    /// <summary>Constructor for the Player class.</summary>
    public Player(bool isDealer=false)
    {
        this._hand = new Hand(betAmount:0, isDealer:isDealer);
    }

}

public class User : Player
{
    /// <summary>The amount of money the player entered the game with</summary>
    public float _startingMoney;

    /// <summary>The amount of money the player currently possesses</summary>
    public float _currentMoney;

    public int _numWins;
    public int _numLosses;
    public int _numTies;

    /// <summary>
    /// User subclass of Player. Tracks current money, starting money, and number of wins and losses.
    /// </summary>
    /// <param name="startingMoney">How much money the player starts with (lesser value = more difficult).</param>
    public User(float startingMoney = 15.0f) : base(false)
    {
        this._startingMoney = startingMoney;
        this._currentMoney = startingMoney;

        _numWins = 0;
        _numLosses = 0;
        _numTies = 0;
    }

    public float GetCurrentEarnings()
    {
        return _currentMoney - _startingMoney;
    }
    public string GetRecord()
    {
        return $"{_numWins}-{_numLosses}-{_numTies}";
    }
    public void PrintRecord_Colored(bool doNewLine = false)
    {
        ConsoleColor winColor = ConsoleColor.Green;
        ConsoleColor lossColor = ConsoleColor.Red;
        ConsoleColor tieColor = ConsoleColor.Yellow;

        Console.ForegroundColor = winColor;
        Console.Write(_numWins);
        Console.ResetColor();
        Console.Write("-");

        Console.ForegroundColor = lossColor;
        Console.Write(_numLosses);
        Console.ResetColor();
        Console.Write("-");

        Console.ForegroundColor = tieColor;
        Console.Write(_numTies);
        Console.ResetColor();

        if (doNewLine)
            Console.WriteLine();
    }
}

/// <summary>
/// 
/// </summary>
public class Dealer : Player
{
    public bool _doHideFirstCard;
    public Dealer(bool doHideFirst=true) : base(true)
    {
        _doHideFirstCard = doHideFirst;
    }
}