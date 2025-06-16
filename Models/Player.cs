namespace BlackjackGame.Models;

/// <summary>
/// Represents a player, 
/// </summary>
public class Player
{
    /// <summary>The player's current hand this round</summary>
    public Hand _hand;

    /// <summary>Constructor for the Player class.</summary>
    public Player(bool isDealer=false)
    {
        this._hand = new Hand(isDealer:isDealer);
    }

}

public class User : Player
{
    /// <summary>The amount of money the player entered the game with</summary>
    public int _startingMoney;

    /// <summary>The amount of money the player currently possesses</summary>
    public int _currentMoney;

    public int _numWins;
    public int _numLosses;
    public int _numTies;

    /// <summary>
    /// User subclass of Player. Tracks current money, starting money, and number of wins and losses.
    /// </summary>
    /// <param name="startingMoney">How much money the player starts with (lesser value = more difficult).</param>
    public User(int startingMoney = 15) : base(false)
    {
        this._startingMoney = startingMoney;
        this._currentMoney = startingMoney;

        _numWins = 0;
        _numLosses = 0;
        _numTies = 0;
    }

    public int GetCurrentEarnings()
    {
        return _currentMoney - _startingMoney;
    }
    public string GetRecord()
    {
        return $"{_numWins}-{_numLosses}-{_numTies}";
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