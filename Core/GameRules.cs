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

    // Customizeable attributes (vary on selection or difficulty)
    public static int ScoreDealerStop = 17;
    public static float WinRatioNormal = 1f;
    public static float WinRatioNaturalBlackjack = 3.0f / 2;
    public static float MinimumBet = 5.0f;
    public static float SurrenderReturnRatio = 0.5f;
    

    public static float UserStartingMoney;




    /// <summary>
    /// Checks if the given hand is a blackjack (score of 21).
    /// </summary>
    /// <param name="hand">The hand to check.</param>
    /// <returns>True if the hand is a blackjack, false otherwise.</returns>
    public static bool CheckHandForBlackjack(Hand hand) => hand.CurrentScore == 21;
    // Settings file path
    private static readonly string SettingsFilePath = "GameRulesSettings.json";

    public class GameRulesSettings
    {
        public int ScoreDealerStop { get; set; } = 17;
        public float WinRatioNormal { get; set; } = 1f;
        public float WinRatioNaturalBlackjack { get; set; } = 1.5f;
        public float MinimumBet { get; set; } = 5.0f;
        public float SurrenderReturnRatio { get; set; } = 0.5f;
    }

    public static void LoadSettings(string? path = null)
    {
        path ??= SettingsFilePath;
        if (!System.IO.File.Exists(path)) return;
        var json = System.IO.File.ReadAllText(path);
        var settings = System.Text.Json.JsonSerializer.Deserialize<GameRulesSettings>(json);
        if (settings != null)
        {
            ScoreDealerStop = settings.ScoreDealerStop;
            WinRatioNormal = settings.WinRatioNormal;
            WinRatioNaturalBlackjack = settings.WinRatioNaturalBlackjack;
            MinimumBet = settings.MinimumBet;
            SurrenderReturnRatio = settings.SurrenderReturnRatio;
        }
    }

    public static void SaveSettings(string? path = null)
    {
        path ??= SettingsFilePath;
        var settings = new GameRulesSettings
        {
            ScoreDealerStop = ScoreDealerStop,
            WinRatioNormal = WinRatioNormal,
            WinRatioNaturalBlackjack = WinRatioNaturalBlackjack,
            MinimumBet = MinimumBet,
            SurrenderReturnRatio = SurrenderReturnRatio
        };
        var json = System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(path, json);
    }



    /// <summary>
    /// Checks for blackjack in either the user or dealer hand, and handles win/loss/tie logic accordingly.
    /// </summary>
    /// <param name="ui">The UI interface for displaying results.</param>
    /// <param name="user">The user player.</param>
    /// <param name="dealer">The dealer player.</param>
    /// <param name="wouldBeNatural">If true, treats a user blackjack as a natural blackjack for payout.</param>
    /// <returns>True if a blackjack was found, false otherwise.</returns>
    public static bool CheckForBlackjack(IGameUI ui, User user, Dealer dealer, bool wouldBeNatural = false)
    {
        bool hasBlackjack_User = GameRules.CheckHandForBlackjack(user.Hand);
        bool hasBlackjack_Dealer = GameRules.CheckHandForBlackjack(dealer.Hand);
        // if blackjack exists at all in either/any hand
        if (hasBlackjack_User || hasBlackjack_Dealer)
        {
            if (hasBlackjack_Dealer)
            {   // show the dealer's card if they get natural blackjack
                dealer.DoHideFirstCard = false;
                if (hasBlackjack_User)
                    GameRules.ResultTie(ui, user);  // BOTH (tie) 
                else
                    GameRules.ResultLose(ui, user, isNatural: wouldBeNatural); // Dealer (lose)
            }
            else // User (win) 
                GameRules.ResultWin(ui, user, isNatural: wouldBeNatural);
            // natural blackjack win means additional earnings

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
    public static bool DealerShouldHit(Dealer dealer) { return dealer.Hand.CurrentScore < ScoreDealerStop; }

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
        float winRatio = (isNatural ? WinRatioNaturalBlackjack : WinRatioNormal);
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
    /// <param name="isNatural">Whether the loss was to a natural blackjack</param>
    public static void ResultLose(IGameUI ui, User user, bool isNatural = false)
    {
        // Don't return the bet amount, lost to the house
        user.NumLosses++;
        user.WinningsRecord.Add(-1 * user.Hand.BetAmount);
        ui.ResultMessage_Loss(user, isNatural);
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