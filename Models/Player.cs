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
    int startingMoney;

    /// <summary>The amount of money the player currently possesses</summary>
    int currentMoney;

    int numWins;

    int numLoses;

    public User(int startingMoney = 15) : base(false)
    {
        this.startingMoney = startingMoney;
        this.currentMoney = startingMoney;
    }
}

/// <summary>
/// 
/// </summary>
public class Dealer : Player
{
    public Dealer() : base(true)
    {
        
    }
}