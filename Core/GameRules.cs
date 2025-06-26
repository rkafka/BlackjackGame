using System.IO.Pipelines;
using System.Text;
using BlackjackGame.Models;
using BlackjackGame.UI;

namespace BlackjackGame.Core;

/// <summary>
/// Contains static rules and logic for Blackjack gameplay.
/// </summary>
public static class GameRules
{
    public const int SCORE_BLACKJACK = 21;
    public const int SCORE_DEALER_STOP = 17;
    public const float WIN_RATIO_NORMAL = 1f;
    public const float WIN_RATIO_NATURAL_BLACKJACK = 3.0f / 2;
    public const float MINIMUM_BET = 5.0f;

    /// <summary>
    /// Checks if the given hand is a blackjack (score of 21).
    /// </summary>
    /// <param name="hand">The hand to check.</param>
    /// <returns>True if the hand is a blackjack, false otherwise.</returns>
    public static bool CheckForBlackjack(Hand hand) => hand.CurrentScore == 21;

    /// <summary>
    /// Checks for blackjack in either the user or dealer hand, and handles win/loss/tie logic accordingly.
    /// </summary>
    /// <param name="ui">The UI interface for displaying results.</param>
    /// <param name="user">The user player.</param>
    /// <param name="dealer">The dealer player.</param>
    /// <param name="wouldBeNatural">If true, treats a user blackjack as a natural blackjack for payout.</param>
    /// <returns>True if a blackjack was found, false otherwise.</returns>
    public static bool CheckForBlackjack(IGameUI ui, User user, Dealer dealer, bool wouldBeNatural=false)
    {
        bool hasBlackjack_User = GameRules.CheckForBlackjack(user.Hand);
        bool hasBlackjack_Dealer = GameRules.CheckForBlackjack(dealer.Hand);
        // if blackjack exists at all in either/any hand
        if (hasBlackjack_User || hasBlackjack_Dealer)
        {
            if (hasBlackjack_User && hasBlackjack_Dealer)   // BOTH (tie)
                GameRules.ResultTie(ui, user);
            else if (hasBlackjack_User)                     // User (win) 
                GameRules.ResultWin(ui, user, isNatural:wouldBeNatural);   // natural blackjack win means additional earnings
            else // hasBlackjack_dealer                     // Dealer (lose)
                GameRules.ResultLose(ui, user);
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Checks if the given hand is a bust (value over 21).
    /// </summary>
    /// <param name="hand">The hand to check.</param>
    /// <returns>True if the hand is a bust, false otherwise.</returns>
    public static bool CheckForBust(Hand hand) { return (hand.CurrentScore > 21); }

    /// <summary>
    /// Determines if the dealer should hit based on their current score.
    /// </summary>
    /// <param name="dealer">The dealer player.</param>
    /// <returns>True if the dealer should hit, false otherwise.</returns>
    public static bool DealerShouldHit(Dealer dealer) { return dealer.Hand.CurrentScore < SCORE_DEALER_STOP; }

    /// <summary>
    /// Determines the winner of the round based on user and dealer hand scores, and updates records and UI.
    /// </summary>
    /// <param name="ui">The UI interface for displaying results.</param>
    /// <param name="user">The user player.</param>
    /// <param name="dealer">The dealer player.</param>
    public static void DecideWinner(IGameUI ui, User user, Dealer dealer)
    {
        bool userBusted = (user.Hand.CurrentScore > 21);
        bool dealerBusted = (dealer.Hand.CurrentScore > 21);
        if (userBusted || (user.Hand.CurrentScore < dealer.Hand.CurrentScore && !dealerBusted))
        {
            ResultLose(ui, user);
        }
        else if (dealerBusted || user.Hand.CurrentScore > dealer.Hand.CurrentScore)
        {
            ResultWin(ui, user);
        }
        else
        {
            ResultTie(ui, user);
        }
        ui.PromptToContinue();
    }

    /// <summary>
    /// Handles the logic for a user win, updating money, win count, and displaying the result.
    /// </summary>
    /// <param name="ui">The UI interface for displaying results.</param>
    /// <param name="user">The user player.</param>
    /// <param name="isNatural">If true, applies natural blackjack payout.</param>
    public static void ResultWin(IGameUI ui, User user, bool isNatural = false)
    {
        // Return the bet amount and winnings (ratio of bet to winnings )
        float winRatio = (isNatural ? WIN_RATIO_NATURAL_BLACKJACK : WIN_RATIO_NORMAL);
        float winnings = user.Hand.BetAmount * winRatio;
        user.CurrentMoney += user.Hand.BetAmount + winnings;
        user.NumWins++;
        user.WinningsRecord.Add(winnings);
        ui.ResultMessage_Win(user, isNatural);
    }
    /// <summary>
    /// Handles the logic for a user loss, updating loss count and displaying the result.
    /// </summary>
    /// <param name="ui">The UI interface for displaying results.</param>
    /// <param name="user">The user player.</param>
    public static void ResultLose(IGameUI ui, User user)
    {
        // Don't return the bet amount, lost to the house
        user.NumLosses++;
        user.WinningsRecord.Add(-1 * user.Hand.BetAmount);
        ui.ResultMessage_Loss(user);
    }
    /// <summary>
    /// Handles the logic for a tie, returning the bet, updating tie count, and displaying the result.
    /// </summary>
    /// <param name="ui">The UI interface for displaying results.</param>
    /// <param name="user">The user player.</param>
    public static void ResultTie(IGameUI ui, User user)
    {
        // return the bet amount back, no winnings
        user.CurrentMoney += user.Hand.BetAmount; 
        user.NumTies++;
        user.WinningsRecord.Add(0);
        ui.ResultMessage_Tie(user);
    }
}