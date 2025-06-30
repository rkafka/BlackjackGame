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
    private int _roundNumber = 0;


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

        // Compute effective values for each card in the hand (Aces as 1 or 11 as needed)
        List<int> effectiveValues = new();
        int handValue = 0;
        List<int> aceIndexes = new();
        for (int i = 0; i < player.Hand.Cards.Count; i++)
        {
            var card = player.Hand.Cards[i];
            if (card.Rank == 1) // Ace
            {
                effectiveValues.Add(11);
                handValue += 11;
                aceIndexes.Add(i);
            }
            else if (card.Rank >= 10)
            {
                effectiveValues.Add(10);
                handValue += 10;
            }
            else
            {
                effectiveValues.Add(card.Rank);
                handValue += card.Rank;
            }
        }
        int aceReduceIdx = aceIndexes.Count - 1;
        while (handValue > 21 && aceReduceIdx >= 0)
        {
            handValue -= 10;
            int aceCardIdx = aceIndexes[aceReduceIdx];
            effectiveValues[aceCardIdx] = 1;
            aceReduceIdx--;
        }

        for (int i = 0; i < player.Hand.Cards.Count; i++)
        {
            Card card = player.Hand.Cards[i];
            if (hideFirstCard && i == 0)
            {
                Console.Write("| [");
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write("HIDDEN");
                UIHelper.ResetConsoleColors();
                Console.WriteLine("]          ?? |");
            }
            else
            {
                // Show the effective value for this card
                string valueStr = effectiveValues.Count > i ? effectiveValues[i].ToString() : Card.GetValue(card.Rank).ToString();
                Console.WriteLine("| " + $"{card.ToString().PadRight(cardPadding) + valueStr}".PadRight(sectionWidth - 4) + " |");
                currentScore += effectiveValues.Count > i ? effectiveValues[i] : card.Value;
            }
        }
        if (hideFirstCard)
            Console.WriteLine($"|________ SCORE: {currentScore}".PadRight(sectionWidth - 5) + "+ ? |");
        else
        {
            string scoreLineStart = "|___________ SCORE: ";
            Console.Write(scoreLineStart);
            if (player.Hand.CurrentScore > 21)
                Console.ForegroundColor = IGameUI.COLOR_BAD;
            else if (player.Hand.CurrentScore == 21)
                Console.ForegroundColor = IGameUI.COLOR_GOOD;
            Console.Write(player.Hand.CurrentScore.ToString().PadRight(sectionWidth - 1 - scoreLineStart.Length));
            UIHelper.ResetConsoleColors();
            Console.WriteLine("|");
        }
        Console.WriteLine();
    }
    /// <summary> Outputs both the user's and dealer's hands to the console in text-based format. </summary>
    /// <param name="user">The user object whose hand(s) to print.</param>
    /// <param name="dealer">The dealer object whose hand(s) to print.</param>
    /// <param name="hideDealersFirstCard">Whether the dealer's first card should be hidden (true on player's turn, false on dealer's turn).</param>
    public void DisplayHands(User user, Dealer dealer, bool hideDealersFirstCard)
    {
        DisplayHand(user);
        Console.WriteLine("VS.");
        DisplayHand(dealer, hideFirstCard: hideDealersFirstCard);
        Console.WriteLine();
    }
    /// <summary> [OVERRIDE] Outputs both the user's and dealer's hands to the console in text-based format. 
    /// If hideDealersFirstCard is not provided, uses dealer.DoHideFirstCard. </summary>
    /// <param name="user">The user object whose hand(s) to print.</param>
    /// <param name="dealer">The dealer object whose hand(s) to print.</param>
    public void DisplayHands(User user, Dealer dealer) { DisplayHands(user, dealer, dealer.DoHideFirstCard); }


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
        UIHelper.PrintUserRecord_WinLoss(engine.User);
        // Optionally, print winnings record if needed
        UIHelper.PrintUserRecord_RoundlyEarnings(engine.User);
    }


    /// <summary>
    /// Prompts the player to select a player action (Hit, Stand, Double Down, Surrender, Split) by typing the associated number.
    /// </summary>
    /// <returns>String containing the input read in from the user.</returns>
    public string PromptPlayerAction(bool isFirstTurn=true, bool canDoubleDown=true)
    {
        Console.Write("PLAYER OPTIONS:  ");
        string[] options = ["Hit", "Stand", "Surrender", "Double Down", "Split"];
        //
        if (!isFirstTurn)
            options = options[..(canDoubleDown ? 2 : 1)];

        for (int i = 0; i < options.Length; i++)
        {
            Console.Write($"[");
            UIHelper.PrintColored(message:(i+1).ToString(), foregroundColor:ConsoleColor.Cyan, doNewLine:false);
            Console.Write($"] {options[i]}  ");
        }
        UIHelper.PrintColored("\n> Type the number to select: ", foregroundColor:IGameUI.COLOR_PROMPT, doNewLine:false);
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
        _roundNumber++;
        string roundMessage = $"Starting Round {_roundNumber}...";
        foreach (char character in roundMessage)
        {
            Console.Write(character);
            Thread.Sleep(30);
        }
        Console.WriteLine();
        Thread.Sleep(120);

        Console.Write($"You currently have {user.CurrentMoney:C2} to gamble with. The ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("minimum bet ");
        UIHelper.ResetConsoleColors();
        Console.Write("is ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"{GameRules.MINIMUM_BET:C0}");
        UIHelper.ResetConsoleColors();
        Console.WriteLine(".");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("How much would you like to bet? Enter as a positive integer -->");
        UIHelper.ResetConsoleColors();
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
        UIHelper.ResetConsoleColors();
        Console.Write($" {(isBet ? "$" : "")}");
    }

    /// <summary> Displays a message indicating the player's chosen action. </summary>
    /// <param name="playerAction">The action chosen by the player.</param>
    public void PlayerAction_ChoiceMessage(string playerAction) { Console.Write($"You chose to {playerAction}.\t"); }

    // public void PlayerAction_HitMessage(User user) { Console.WriteLine($"You drew the {user._hand._cards.Last()} ({user._hand._cards.Last()._value})"); }

    /// <summary> Displays a message indicating that the selected player action is not supported in the current build. </summary>
    public void PlayerAction_NotSupportedMessage() { Console.WriteLine("Sorry, this option is not supported in the current build."); }

    /// <summary> Displays a message indicating the user has won, with special formatting for a natural blackjack. </summary>
    /// <param name="user">The user who won.</param>
    /// <param name="isNatural">If true, indicates a natural blackjack win.</param>
    public void ResultMessage_Win(User user, bool isNatural=false)
    {
        if (isNatural)
        {
            Console.Write("You won with a ");
            UIHelper.PrintSlowly("NATURAL BLACKJACK", msPerChar:60, foregroundColor:IGameUI.COLOR_GOOD);
            Console.WriteLine($"! Your bet of {user.Hand.BetAmount:C0} has been returned along with 1.5x its value in winnings.");
        }
        else
        {
            Console.WriteLine($"You won! Your bet of {user.Hand.BetAmount:C0} has been doubled and returned to you.");
        }
        Console.Write($"Remaining Money:  {user.CurrentMoney:C2}  |  W/L/T Record:  ");
        UIHelper.PrintUserRecord_WinLoss(user);
    }

    /// <summary> Displays a message indicating the user has lost and their remaining money and record. </summary>
    /// <param name="user">The user who lost.</param>
    /// <param name="isNatural"></param>
    public void ResultMessage_Loss(User user, bool isNatural = false)
    {
        if (isNatural)
        {
            Console.Write("Uh Oh! The Dealer got a ");
            UIHelper.PrintSlowly("NATURAL BLACKJACK", msPerChar: 60, foregroundColor: IGameUI.COLOR_BAD);
            Console.WriteLine($"! You lose your bet of {user.Hand.BetAmount:C0}.");
        }
        else
        {
            Console.WriteLine($"You lost... Your bet of {user.Hand.BetAmount:C0} has been lost.");
        }
        Console.Write($"Remaining Money:  {user.CurrentMoney:C2}  |  W/L/T Record:  ");
        UIHelper.PrintUserRecord_WinLoss(user);
    }

    /// <summary> Displays a message indicating the user has tied and their remaining money and record. </summary>
    /// <param name="user">The user who tied.</param>
    public void ResultMessage_Tie(User user)
    {
        Console.WriteLine($"You tied. Your bet of {user.Hand.BetAmount:C0} has been returned to you.");
        Console.Write($"Remaining Money:  {user.CurrentMoney:C2}  |  W/L/T Record:  ");
        UIHelper.PrintUserRecord_WinLoss(user);
    }

    /// <summary> Reveals the dealer's hidden first card and displays both hands. </summary>
    /// <param name="user">The user object.</param>
    /// <param name="dealer">The dealer object.</param>
    public void RevealDealersHiddenCard(User user, Dealer dealer) {
        Console.WriteLine("Your turn is now over. Revealing the Dealer's hidden first card ...");
        DisplayHands(user, dealer, hideDealersFirstCard: false);
    }

}