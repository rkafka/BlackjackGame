using System.IO.Pipelines;
using System.Text;
using BlackjackGame.Models;
using BlackjackGame.UI;

namespace BlackjackGame.Core;

public static class GameRules
{
    // score-based markers
    public const int SCORE_BLACKJACK = 21;
    public const int SCORE_DEALER_STOP = 17;

    // ratios to multiply winnings/losses by
    public const float WIN_RATIO_NORMAL = 1f;
    public const float WIN_RATIO_NATURAL_BLACKJACK = 3.0f/2;

    public const float MINIMUM_BET = 5.0f;

    /// <summary>
    /// Calculates the total value of a hand, treating Aces as 11 or 1 as needed to avoid busting. Updates hand._currentScore.
    /// </summary>
    /// <param name="hand">The hand to evaluate.</param>
    /// <returns>The total value of the hand.</returns>
    public static int CalculateHandValue(Hand hand)
    {
        int handValue = 0;
        List<Card> aces = [];
        foreach (var card in hand._cards)
        {
            if (card._rank == 1) // Ace
            {
                handValue += 11;
                aces.Add(card);
            }
            else if (card._rank >= 10) // Face cards or 10
                handValue += 10;
            else
                handValue += card._rank;
        }
        // Reduce Ace(s) from 11 to 1 as needed to avoid bust
        while (handValue > 21 && aces.Count > 0)
        {
            handValue -= 10;
            aces[^1]._value = 1;
            aces.RemoveAt(aces.Count-1);
        }

        hand._currentScore = handValue;
        return handValue;
    }

    /// <summary>
    /// Checks if the given hand is a blackjack (score of 21).
    /// </summary>
    /// <param name="hand">The hand to check.</param>
    /// <returns>True if the hand is a blackjack, false otherwise.</returns>
    public static bool CheckForBlackjack(Hand hand) { return (hand._currentScore == 21); }

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
        bool hasBlackjack_User = GameRules.CheckForBlackjack(user._hand);
        bool hasBlackjack_Dealer = GameRules.CheckForBlackjack(dealer._hand);
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
    public static bool CheckForBust(Hand hand) { return (CalculateHandValue(hand) > 21); }

    /// <summary>
    /// Determines if the dealer should hit based on their current score.
    /// </summary>
    /// <param name="dealer">The dealer player.</param>
    /// <returns>True if the dealer should hit, false otherwise.</returns>
    public static bool DealerShouldHit(Dealer dealer) { return dealer._hand._currentScore < SCORE_DEALER_STOP; }

    /// <summary>
    /// Determines the winner of the round based on user and dealer hand scores, and updates records and UI.
    /// </summary>
    /// <param name="ui">The UI interface for displaying results.</param>
    /// <param name="user">The user player.</param>
    /// <param name="dealer">The dealer player.</param>
    public static void DecideWinner(IGameUI ui, User user, Dealer dealer)
    {
        bool userBusted = (user._hand._currentScore > 21);
        bool dealerBusted = (dealer._hand._currentScore > 21);

        // for(int i = 0; i < _user._hands.Count; i++)
        if (userBusted || (user._hand._currentScore < dealer._hand._currentScore && !dealerBusted)) // DEALER wins (USER loses)
        {
            ResultLose(ui, user);
        }
        else if (dealerBusted || user._hand._currentScore > dealer._hand._currentScore) // USER wins!!
        {
            ResultWin(ui, user);
        }
        else // TIED
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
        float winnings = user._hand._betAmount * winRatio;
        user._currentMoney += user._hand._betAmount + winnings;
        user._numWins++;
        user.winningsRecord.Add(winnings);
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
        user._numLosses++;
        user.winningsRecord.Add(-1*user._hand._betAmount);
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
        user._currentMoney += user._hand._betAmount; 
        user._numTies++;
        user.winningsRecord.Add(0);
        ui.ResultMessage_Tie(user);
    }
}