using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BlackjackGame.Core;
using BlackjackGame.Models;
using BlackjackGame.Utils;

namespace BlackjackGame.UI;

public class UI_TextBased : IGameUI
{
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

    public void CardDrawnMessage(Player player)
    {
        Card cardDrawn = player._hand._cards.Last();
        string playerName = ((player._hand._isDealer) ? "The Dealer":"You");
        Console.WriteLine($"\n{playerName} drew the {cardDrawn} (value: {Card.GetValue(cardDrawn._rank, cardDrawn._value)})\n");
    }

    public void DisplayTitle() { Console.WriteLine("\n" + Utils.ASCII.ascii_Title); }

    public void GameOverMessage(GameEngine engine)
    {
        UIHelper.PrintSectionHeader("GAME OVER");
        Console.Write("Your final record was ");
        engine._user.PrintRecord_Colored(doNewLine: true);
    }


    /// <summary> Prompts the player to select a Player Action (Hit, Stand, Double Down, etc.) by typing their associated number. </summary>
    /// <returns>String containing the input read in from the user</returns>
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

    /// <summary> Writes "Press enter to continue.." to the Console Terminal and waits for ReadLine() input. 
    /// Clears the  </summary>
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

    public void PromptForBet(User user)
    {
        Console.WriteLine("Starting new round...");

        Console.Write($"You currently have {user._currentMoney:C2} to gamble with. The ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("minimum bet ");
        Console.ResetColor();
        Console.Write("is ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"{GameRules.MINIMUM_BET:C0}");
        Console.ResetColor();
        Console.WriteLine(".");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("How much would you like to bet? Enter as a positive integer -->");
        Console.ResetColor();
        Console.Write(" $");
    }
    public void PromptAfterError(string problem, bool isBet = false)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.Write($"Oops! {problem}, try again: ");
        Console.ResetColor();
        Console.Write($" {(isBet ? "$" : "")}");
    }


    /// <summary> Outputs both the user & dealer hands to the Console Terminal in Text-Based format. </summary>
    /// <param name="user">The user object to print the hand(s) of</param>
    /// <param name="dealer">The dealer object to print the hand(s) of</param>
    /// <param name="hideDealersFirstCard">Whether the dealer's first card should be hidden (true on players turn, false on dealer's turn).</param>
    public void DisplayHands(User user, Dealer dealer, bool hideDealersFirstCard = false)
    {
        // Print user's hand of cards, with scores included
        DisplayHands(user);
        Console.WriteLine("\nVS.");
        // Print dealer's hand of cards, with scores included
        DisplayHands(dealer, hideFirstCard: hideDealersFirstCard);
        Console.WriteLine();
    }

    /// <summary> Outputs a singular hand to the Console Terminal. </summary>
    /// <param name="player">The user or dealer object of which to print the hand</param>
    /// <param name="hideFirstCard">Boolean marker determining whether the first card is hidden (Dealer hides their first card before their turn).</param>
    public void DisplayHands(Player player, bool hideFirstCard = false)
    {
        int cardPadding = 18;
        int sectionWidth = 24;

        string playerName = ((player._hand._isDealer) ? "DEALER" : "USER");
        int currentScore = 0;
        Console.WriteLine("\n" + $"{playerName}'S HAND:".PadRight(sectionWidth - 1, '-') + ".");
        for (int i = 0; i < player._hand._cards.Count; i++) // foreach(Card card in player._hand._cards)
        {
            Card card = player._hand._cards[i];
            // hide first card if its player's turn
            if (hideFirstCard && i == 0)
            {
                Console.Write("| [");
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write("HIDDEN");
                Console.ResetColor();
                Console.WriteLine("]          ?? |");
            }
            else // output card info to hand display
            {
                Console.WriteLine("| " + $"{card.ToString().PadRight(cardPadding) + Card.GetValue(card._rank).ToString()}".PadRight(sectionWidth - 4) + " |");
                currentScore += card._value;
            }
        }
        if (hideFirstCard)
            Console.WriteLine($"|________ SCORE: {currentScore}".PadRight(sectionWidth - 5) + "+ ? |");
        else
        {
            string scoreLineStart = "|___________ SCORE: ";
            Console.Write(scoreLineStart);
            if (player._hand._currentScore > 21)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (player._hand._currentScore == 21)
                Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(player._hand._currentScore.ToString().PadRight(sectionWidth - 1 - scoreLineStart.Length));
            Console.ResetColor();
            Console.WriteLine("|");
        }
        Console.WriteLine(); // extra spacing from next element
    }

    public void PlayerAction_ChoiceMessage(string playerAction) { Console.Write($"You chose to {playerAction}.\t"); }
    // public void PlayerAction_HitMessage(User user) { Console.WriteLine($"You drew the {user._hand._cards.Last()} ({user._hand._cards.Last()._value})"); }
    public void PlayerAction_NotSupportedMessage() { Console.WriteLine("Sorry, this option is not supported in the current build."); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    public void ResultMessage_Win(User user, bool isNatural)
    {
        if (isNatural)
        {
            Console.Write("You won with a ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("NATURAL BLACKJACK");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"! Your bet of {user._hand._betAmount:C0} has been returned along with 1.5x its value in winnings.");
        }
        else
        {
            Console.WriteLine($"You won! Your bet of {user._hand._betAmount:C0} has been doubled and returned to you.");
        }
        Console.Write($"Remaining Money:  {user._currentMoney:C2}  |  W/L/T Record:  ");
        user.PrintRecord_Colored(doNewLine: true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    public void ResultMessage_Loss(User user)
    {
        Console.WriteLine($"You lost... Your bet of {user._hand._betAmount:C0} has been lost.");
        Console.Write($"Remaining Money:  {user._currentMoney:C2}  |  W/L/T Record:  ");
        user.PrintRecord_Colored(doNewLine: true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResultMessage_Tie(User user)
    {
        Console.WriteLine($"You tied. Your bet of {user._hand._betAmount:C0} has been returned to you.");
        Console.Write($"Remaining Money:  {user._currentMoney:C2}  |  W/L/T Record:  ");
        user.PrintRecord_Colored(doNewLine: true);
    }

    public void RevealDealersHiddenCard(User user, Dealer dealer) {
        Console.WriteLine("Your turn is now over. Revealing the Dealer's hidden first card ...");
        DisplayHands(user, dealer, hideDealersFirstCard: false);
    }

}