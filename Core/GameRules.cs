using System.IO.Pipelines;
using BlackjackGame.Models;
using BlackjackGame.UI;

namespace BlackjackGame.Core;

public static class GameRules
{
    // score-based markers
    public const int SCORE_BLACKJACK = 21;
    public const int SCORE_DEALER_STOP = 17;

    // ratios to multiply winnings/losses by
    public const float WIN_RATIO = 3.0f/2;
    public const float LOSS_RATIO = 1;

    public const float MINIMUM_BET = 5.0f;

    public static int CalculateHandValue(Hand hand)
    {
        throw new NotImplementedException();
    }

    public static bool CheckForBlackjack(Hand hand) { return (hand._currentScore == 21); }
    public static bool CheckForBlackjack(IGameUI ui, User user, Dealer dealer)
    {
        bool hasBlackjack_User = GameRules.CheckForBlackjack(user._hand);
        bool hasBlackjack_Dealer = GameRules.CheckForBlackjack(dealer._hand);
        // if blackjack exists at all in either/any hand
        if (hasBlackjack_User || hasBlackjack_Dealer)
        {
            if (hasBlackjack_User && hasBlackjack_Dealer)   // BOTH (tie)
                GameRules.ResultTie(ui, user);
            else if (hasBlackjack_User)                     // User (win) 
                GameRules.ResultWin(ui, user);
            else // hasBlackjack_dealer                     // Dealer (lose)
                GameRules.ResultLose(ui, user);
            return true;
        }
        else
            return false;
    }

    public static bool CheckForBust(Hand hand)
    {
        if (hand._currentScore > 21)
        {
            // while(hand._currentScore > 21) // TO-DO: check for aces to revalue            
            Console.WriteLine("BUST. The cards cumulative value is greater than 21.");
            return false;
        }
        return true;
    }

    public static bool DealerShouldHit(Dealer dealer) { return dealer._hand._currentScore < SCORE_DEALER_STOP; }

    public static void ResultWin(IGameUI ui, User user)
    {
        user._currentMoney += user._hand._betAmount * WIN_RATIO;
        user._numWins++;
        ui.ResultMessage_Win(user);
    }
    public static void ResultLose(IGameUI ui, User user)
    {
        user._currentMoney -= user._hand._betAmount;
        user._numLosses++;
        ui.ResultMessage_Loss(user);
    }
    public static void ResultTie(IGameUI ui, User user)
    {
        user._currentMoney += user._hand._betAmount;
        user._numTies++;
        ui.ResultMessage_Tie(user);
    }
}