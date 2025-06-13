namespace BlackjackGame.Models;

public class Player
{
    int startingMoney;
    int currentMoney;
    int currentBet;
    int currentScore;
    Hand currentHand;

    public Player(int startingMoney = 15)
    {
        this.startingMoney = startingMoney;
        this.currentMoney = startingMoney;

        this.currentBet = 0;
        this.currentScore = 0;
        this.currentHand = new();
    }

}