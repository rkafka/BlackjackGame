using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BlackjackGame.Core;
using BlackjackGame.Models;
using BlackjackGame.Utils;

namespace BlackjackGame.UI;

/// <summary>
/// Text-based UI implementation for Blackjack.
/// </summary>
public class UI_TextBased : IGameUI
{
    /// <summary>
    /// Displays the launch screen with the title and hand art, and optionally waits for user input before clearing the screen.
    /// </summary>
    /// <param name="waitForInput">If true, waits for the user to press enter before clearing the screen.</param>
    public void LaunchScreen(bool waitForInput = true)
    {
        Console.Clear();
        DisplayTitle();

        int middleWhiteSpace = Console.WindowHeight - (Utils.ASCII.ascii_Title.Split("\n").Length + 1);
        middleWhiteSpace -= Utils.ASCII.ascii_HandCropped.Split("\n").Length;
        // if (middleWhiteSpace > ascii_PressEnterToStart.Split("\n").Length + 2)
        // {
        //     middleWhiteSpace -= (ascii_PressEnterToStart.Split("\n").Length + 2);
        //     Console.Write("".PadLeft(middleWhiteSpace / 2 + middleWhiteSpace % 2, '\n'));
        //     middleWhiteSpace /= 2;
        //     Console.Write(ascii_PressEnterToStart);
        // }
        Console.Write("".PadRight((middleWhiteSpace > 0 ? middleWhiteSpace : 0), '\n'));

        Console.Write(Utils.ASCII.ascii_HandCropped);
        if (waitForInput) { Console.ReadLine(); }
        Console.Clear();
    }

    /// <summary>
    /// Displays a message indicating which player drew which card, including the card's value.
    /// </summary>
    /// <param name="player">The player who drew the card.</param>
    public void CardDrawnMessage(Player player)
    {
        Card cardDrawn = player.Hand.Cards.Last();
        string playerName = (player.Hand.IsDealer ? "The Dealer" : "You");
        Console.WriteLine($"\n{playerName} drew the {cardDrawn} (value: {Card.GetValue(cardDrawn.Rank, cardDrawn.Value)})\n");
    }


    /// <summary>
    /// Outputs both the user's and dealer's hands to the console in text-based format.
    /// </summary>
    /// <param name="user">The user object whose hand(s) to print.</param>
    /// <param name="dealer">The dealer object whose hand(s) to print.</param>
    /// <param name="hideDealersFirstCard">Whether the dealer's first card should be hidden (true on player's turn, false on dealer's turn).</param>
    public void DisplayHands(User user, Dealer dealer, bool hideDealersFirstCard = false)
    {
        GameRules.CalculateHandValue(user.Hand);
        DisplayHand(user);
        Console.WriteLine("\nVS.");
        GameRules.CalculateHandValue(dealer.Hand);
        DisplayHand(dealer, hideFirstCard: hideDealersFirstCard);
        Console.WriteLine();
    }

    /// <summary>
    /// Outputs a single player's hand to the console, optionally hiding the first card (for the dealer).
    /// </summary>
    /// <param name="player">The player whose hand to print.</param>
    /// <param name="hideFirstCard">If true, hides the first card (for the dealer before their turn).</param>
    public void DisplayHand(Player player, bool hideFirstCard = false)
    {
        int cardPadding = 18;
        int sectionWidth = 24;
        string playerName = (player.Hand.IsDealer ? "DEALER" : "USER");
        int currentScore = 0;
        Console.WriteLine("\n" + $"{playerName}'S HAND:".PadRight(sectionWidth - 1, '-') + ".");
        for (int i = 0; i < player.Hand.Cards.Count; i++)
        {
            Card card = player.Hand.Cards[i];
            if (hideFirstCard && i == 0)
            {
                Console.Write("| [");
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write("HIDDEN");
                Console.ResetColor();
                Console.WriteLine("]          ?? |");
            }
            else
            {
                Console.WriteLine("| " + $"{card.ToString().PadRight(cardPadding) + Card.GetValue(card.Rank).ToString()}".PadRight(sectionWidth - 4) + " |");
                currentScore += card.Value;
            }
        }
        if (hideFirstCard)
            Console.WriteLine($"|________ SCORE: {currentScore}".PadRight(sectionWidth - 5) + "+ ? |");
        else
        {
            string scoreLineStart = "|___________ SCORE: ";
            Console.Write(scoreLineStart);
            if (player.Hand.CurrentScore > 21)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (player.Hand.CurrentScore == 21)
                Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(player.Hand.CurrentScore.ToString().PadRight(sectionWidth - 1 - scoreLineStart.Length));
            Console.ResetColor();
            Console.WriteLine("|");
        }
        Console.WriteLine();
    }


    /// <summary>
    /// Displays the ASCII art title to the console.
    /// </summary>
    public void DisplayTitle() { Console.WriteLine("\n" + Utils.ASCII.ascii_Title); }

    /// <summary>
    /// Displays the game over message and the user's final record.
    /// </summary>
    /// <param name="engine">The game engine instance containing the user record.</param>
    public void GameOverMessage(GameEngine engine)
    {
        UIHelper.PrintSectionHeader("GAME OVER");
        Console.Write("Your final record was ");
        Console.WriteLine(engine.User.GetWinLossRecord());
        // Optionally, print winnings record if needed
    }


    /// <summary>
    /// Prompts the player to select a player action (Hit, Stand, Double Down, Surrender, Split) by typing the associated number.
    /// </summary>
    /// <returns>String containing the input read in from the user.</returns>
    public string PromptPlayerAction()
    {
        Console.Write("PLAYER OPTIONS:  ");
        string[] options = ["Hit", "Stand", "Double Down", "Surrender", "Split"];
        for (int i = 0; i < options.Length; i++)
        {
            Console.Write($"[");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{i + 1}");
            Console.ResetColor();
            Console.Write($"] {options[i]}  ");
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n> Type the number to select: ");
        Console.ResetColor();
        return (Console.ReadLine() ?? "").Trim().ToLower();
        // ?? ""   <-- means you should return an empty string if the input is null
    }

    /// <summary>
    /// Writes "Press enter to continue..." to the console and waits for user input, then clears the prompt from the screen.
    /// </summary>
    public void PromptToContinue()
    {
        int startY = Console.CursorTop;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Press enter to continue...");

        Console.ForegroundColor = IGameUI.COLOR_DEFAULT_FOREGROUND;
        Console.ReadLine();

        Console.SetCursorPosition(0, startY-1);
        // erase the lines written
        for (int i = 0; i < 2; i++) { Console.Write(new string(' ', Console.WindowWidth)); }
        Console.SetCursorPosition(0, startY-1);
    }

    /// <summary>
    /// Prompts the user to enter a bet amount, displaying their current money and the minimum bet.
    /// </summary>
    /// <param name="user">The user to prompt for a bet.</param>
    public void PromptForBet(User user)
    {
        Console.WriteLine("Starting new round...");
        Console.Write($"You currently have {user.CurrentMoney:C2} to gamble with. The ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("minimum bet ");
        Console.ResetColor();
        Console.Write("is ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"{GameRules.MinimumBet:C0}");
        Console.ResetColor();
        Console.WriteLine(".");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("How much would you like to bet? Enter as a positive integer -->");
        Console.ResetColor();
        Console.Write(" $");
    }
    /// <summary>
    /// Displays an error message in red and prompts the user to try again, optionally indicating a bet input.
    /// </summary>
    /// <param name="problem">The error message to display.</param>
    /// <param name="isBet">If true, displays a dollar sign for bet input.</param>
    /// <param name="tryAgain">If true, prompts the user to try again.</param>
    public void PromptAfterError(string problem, bool isBet = false, bool tryAgain = true)
    {
        // ResetConsoleColors();
        // Thread.Sleep(5);

        Console.BackgroundColor = ConsoleColor.Red;
        Console.Write($"Oops! {problem}");
        Console.Write((tryAgain) ? ", try again: " : ".\n");
        Console.ResetColor();
        Console.Write($" {(isBet ? "$" : "")}");
    }

    /// <summary> Displays a message indicating the player's chosen action. </summary>
    /// <param name="playerAction">The action chosen by the player.</param>
    public void PlayerAction_ChoiceMessage(string playerAction) { Console.Write($"You chose to {playerAction}.\t"); }

    // public void PlayerAction_HitMessage(User user) { Console.WriteLine($"You drew the {user._hand._cards.Last()} ({user._hand._cards.Last()._value})"); }

    /// <summary> Displays a message indicating that the selected player action is not supported in the current build. </summary>
    public void PlayerAction_NotSupportedMessage() { Console.WriteLine("Sorry, this option is not supported in the current build."); }

    /// <summary> Resets the console's background and foreground colors to their default values. </summary>
    public void ResetConsoleColors()
    {
        Console.BackgroundColor = Utils.ASCII.DEFAULT_BACKGROUND;
        Console.ForegroundColor = Utils.ASCII.DEFAULT_FOREGROUND;
    }


    /// <summary> Displays a message indicating the user has won, with special formatting for a natural blackjack. </summary>
    /// <param name="user">The user who won.</param>
    /// <param name="isNatural">If true, indicates a natural blackjack win.</param>
    public void ResultMessage_Win(User user, bool isNatural=false)
    {
        if (isNatural)
        {
            Console.Write("You won with a ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("NATURAL BLACKJACK");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"! Your bet of {user.Hand.BetAmount:C0} has been returned along with 1.5x its value in winnings.");
        }
        else
        {
            Console.WriteLine($"You won! Your bet of {user.Hand.BetAmount:C0} has been doubled and returned to you.");
        }
        Console.Write($"Remaining Money:  {user.CurrentMoney:C2}  |  W/L/T Record:  ");
        Console.WriteLine(user.GetWinLossRecord());
    }

    /// <summary> Displays a message indicating the user has lost and their remaining money and record. </summary>
    /// <param name="user">The user who lost.</param>
    public void ResultMessage_Loss(User user)
    {
        Console.WriteLine($"You lost... Your bet of {user.Hand.BetAmount:C0} has been lost.");
        Console.Write($"Remaining Money:  {user.CurrentMoney:C2}  |  W/L/T Record:  ");
        Console.WriteLine(user.GetWinLossRecord());
    }

    /// <summary> Displays a message indicating the user has tied and their remaining money and record. </summary>
    /// <param name="user">The user who tied.</param>
    public void ResultMessage_Tie(User user)
    {
        Console.WriteLine($"You tied. Your bet of {user.Hand.BetAmount:C0} has been returned to you.");
        Console.Write($"Remaining Money:  {user.CurrentMoney:C2}  |  W/L/T Record:  ");
        Console.WriteLine(user.GetWinLossRecord());
    }

    /// <summary> Reveals the dealer's hidden first card and displays both hands. </summary>
    /// <param name="user">The user object.</param>
    /// <param name="dealer">The dealer object.</param>
    public void RevealDealersHiddenCard(User user, Dealer dealer) {
        Console.WriteLine("Your turn is now over. Revealing the Dealer's hidden first card ...");
        DisplayHands(user, dealer, hideDealersFirstCard: false);
    }

}