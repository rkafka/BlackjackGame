namespace BlackjackGame.Models;

/// <summary>
/// Represents a player in the Blackjack game.
/// </summary>
public class Player
{
    /// <summary>
    /// The player's current hand this round.
    /// </summary>
    private Hand _hand;

    /// <summary>
    /// Gets or sets the player's current hand.
    /// </summary>
    public Hand Hand
    {
        get => _hand;
        set => _hand = value;
    }

    public const float StartingMoneyEasy = 30.0f;
    public const float StartingMoneyNormal = 30.0f;
    public const float StartingMoneyHard = 30.0f;

    /// <summary>
    /// Constructor for the Player class.
    /// </summary>
    public Player(bool isDealer = false)
    {
        _hand = new Hand(betAmount: 0, isDealer: isDealer);
    }
}

/// <summary>
/// Represents a user (human player) in the Blackjack game.
/// </summary>
/// <remarks> User subclass of Player. Tracks current money, starting money, and number of wins and losses. </remarks>
/// <param name="startingMoney">How much money the player starts with (lesser value = more difficult).</param>
public class User(float startingMoney = 15.0f) : Player(false)
{
    private float _startingMoney = startingMoney;
    private float _currentMoney = startingMoney;
    private int _numWins = 0;
    private int _numLosses = 0;
    private int _numTies = 0;
    private List<float> _winningsRecord = [];

    /// <summary> Gets the amount of money the player entered the game with. </summary>
    public float StartingMoney => _startingMoney;

    /// <summary> Gets or sets the amount of money the player currently possesses. </summary>
    public float CurrentMoney
    {  get => _currentMoney;  set => _currentMoney = value;  }

    /// <summary> Gets or sets the number of wins. </summary>
    public int NumWins
    {  get => _numWins;  set => _numWins = value;  }

    /// <summary> Gets or sets the number of losses. </summary>
    public int NumLosses
    {  get => _numLosses;  set => _numLosses = value;  }

    /// <summary> Gets or sets the number of ties. </summary>
    public int NumTies
    {  get => _numTies;  set => _numTies = value;  }

    /// <summary> Gets the record of winnings for the user. </summary>
    public List<float> WinningsRecord => _winningsRecord;



    /* FUNCTIONS */

    /// <summary> Gets the current earnings of the user. </summary>
    /// <returns>the difference between the user's current money and the amount they started playing with.</returns>
    public float GetCurrentEarnings()
    {
        return _currentMoney - _startingMoney;
    }

    /// <summary> Gets the win/loss/tie record as a string. </summary>
    /// <returns>a string of the win-loss record in W-L-T format</returns>
    public string GetWinLossRecord()
    {
        return $"{_numWins}-{_numLosses}-{_numTies}";
    }
}

/// <summary> Represents the dealer in the Blackjack game. Subclass of <see cref="Player"/>. </summary>
public class Dealer : Player
{
    private bool _doHideFirstCard;

    /// <summary>
    /// Gets or sets whether the dealer's first card should be hidden.
    /// </summary>
    public bool DoHideFirstCard
    {
        get => _doHideFirstCard;
        set => _doHideFirstCard = value;
    }

    /// <summary>
    /// Initializes a new instance of the Dealer class.
    /// </summary>
    /// <param name="doHideFirst">Whether the dealer's first card should be hidden.</param>
    public Dealer(bool doHideFirst = true) : base(true)
    {
        _doHideFirstCard = doHideFirst;
    }
}