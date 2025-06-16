using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BlackjackGame.Utils;

namespace BlackjackGame.Models;

public class Game
{
    public const ConsoleColor COLOR_F_USER = ConsoleColor.Blue;
    public const ConsoleColor COLOR_B_USER = ConsoleColor.DarkBlue;
    public const ConsoleColor COLOR_F_DEALER = ConsoleColor.Green;
    public const ConsoleColor COLOR_B_DEALER = ConsoleColor.DarkGreen;


    public const int SCORE_BLACKJACK = 21;
    public const int SCORE_SOFT_BLACKJACK = 17;

    //
    public Deck _deck;
    public User _user;
    public Dealer _dealer;

    public Game(Deck deck, User user, Dealer dealer)
    {
        _deck = deck;
        _user = user;
        _dealer = dealer;
    }

    public void InitialDraw()
    {
        _user._hand.AddCard(_deck);
        _user._hand.AddCard(_deck);
        _dealer._hand.AddCard(_deck);
        _dealer._hand.AddCard(_deck);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// Status code denoting if someone has a blackjack. '0' for not found,
    /// '1' for user blackjack, '2' for dealer blackjack, or '3' for both
    /// </returns>
    public int BlackjackCheck()
    {
        if (_user._hand._currentScore == SCORE_BLACKJACK)
        {
            if (_dealer._hand._currentScore == SCORE_BLACKJACK)
                return 3;   // BOTH have blackjack, no winner reset round
            return 1;   // USER wins!
        }
        else if (_dealer._hand._currentScore == SCORE_BLACKJACK)
        {   // DEALER wins!
            return 2;
        }
        else
        {   // nobody wins, continue
            return 0;
        }
    }

    /// <summary>
    /// 3. Player Actions
    /// - For each hand, players choose:
    ///     Hit – take another card
    ///     Stand – keep current hand
    ///     Double Down – double your bet, take one card only
    ///     Split – if you have a pair, split into 2 hands (each gets another card) [NOT SUPPORTED]
    ///     Surrender – (if allowed) forfeit half your bet and end the hand
    /// - Players can continue hitting until they stand or bust (go over 21).
    /// </summary>
    /// <returns></returns>
    public int PlayerActions()
    {
        string playerAction;
        int playerChoiceNum = -1;

        bool keepDrawing = true;
        while (BlackjackCheck() == 0 && keepDrawing)
        {
            Console.Write("PLAYER OPTIONS: [1] Hit  [2] Stand  [3] Double Down   |   Type the number to select:  ");//, [4] Split, [5] Surrender");
            if (!int.TryParse(Console.ReadLine(), out int playerChoice))
                continue;
            switch (playerChoice)
            {
                case 1:
                    // HIT
                    playerAction = "Hit";
                    Console.Write($"You chose to {playerAction}.\t");
                    _user._hand.AddCard(_deck);
                    Console.WriteLine($"You drew the {_user._hand._cards.Last()} ({_user._hand._cards.Last()._value})");
                    PrintAllHandsAsText();
                    break;
                case 2:
                    // STAND
                    playerAction = "Stand";
                    Console.WriteLine($"You chose to {playerAction}.\tProceeding to dealer's turn.");
                    keepDrawing = false;
                    break;
                case 3:
                    // DOUBLE DOWN
                    playerAction = "Double Down";
                    Console.WriteLine($"You chose to {playerAction}. ");
                    Console.WriteLine("ERROR: 'Double Down' option not currently supported."); // TO-DO
                    break;
                case 4:
                    // SPLIT
                    playerAction = "Split";
                    Console.WriteLine($"You chose to {playerAction}. ");
                    Console.WriteLine("ERROR: 'Split' option not currently supported."); // TO-DO
                    break;
                case 5:
                    // SURRENDER
                    playerAction = "Surrender";
                    Console.WriteLine($"You chose to {playerAction}. ");
                    Console.WriteLine("ERROR: 'Surrender' option not currently supported."); // TO-DO
                    break;
                default:
                    // unknown input
                    Console.WriteLine("Your input was not an integer in the correct range (1-5). Please try again.");
                    break;
            }
        }
        Console.WriteLine();
        return playerChoiceNum;
    }

    public void UpdateHands()
    {
        UI_Hands(); //(_user, _dealer);
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.WriteLine();
    }

    public void UI_Hands()//(User user, Dealer dealer)
    {
        int distanceFromBottom = 10;
        int yCoord_UserLine = Console.WindowHeight - distanceFromBottom;
        Console.SetCursorPosition(0, yCoord_UserLine);
        Console.Write("".PadRight(Console.WindowWidth, '_'));

        // Make sure the space representing the user's hand area is cleared
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Write("".PadRight(Console.WindowWidth * (distanceFromBottom - 3), ' '));
        Console.BackgroundColor = ASCII.DEFAULT_BACKGROUND;

        // USER TITLE
        Console.SetCursorPosition(0, yCoord_UserLine - Utils.ASCII.ascii_User.Split('\n').Length);
        Utils.ASCII.DisplayASCII(Utils.ASCII.ascii_User, leftToRight: true, foregroundColor: COLOR_F_USER);
        // USER HAND
        Console.SetCursorPosition(0, yCoord_UserLine + 1);
        for (int i = 0; i < _user._hand._cards.Count; i++)
        {
            Utils.ASCII.DisplayASCII(_user._hand._cards[i].GetASCII(), true, backgroundColor: COLOR_B_USER);
        }
        // RESET BACKGROUND COLOR
        Console.BackgroundColor = ASCII.DEFAULT_BACKGROUND;



        int yCoord_DealerLine = yCoord_UserLine - Utils.ASCII.ascii_User.Split('\n').Length - 1;
        Console.SetCursorPosition(0, yCoord_DealerLine);

        Console.Write("".PadRight(Console.WindowWidth, '_'));
        // Make sure the space representing the dealer's hand area is cleared
        Console.BackgroundColor = COLOR_B_DEALER;
        Console.SetCursorPosition(0, yCoord_DealerLine - (distanceFromBottom - 3));
        Console.Write("".PadRight(Console.WindowWidth * (distanceFromBottom - 3), ' '));
        Console.BackgroundColor = ASCII.DEFAULT_BACKGROUND;

        // DEALER TITLE
        int x = (Console.WindowWidth - (Utils.ASCII.ascii_Dealer.Split('\n')[0]).Length);
        int y = (yCoord_UserLine - Utils.ASCII.ascii_User.Split('\n').Length);
        Console.SetCursorPosition(x, y);
        Utils.ASCII.DisplayASCII(Utils.ASCII.ascii_Dealer, leftToRight: false, foregroundColor: COLOR_F_DEALER);
        // DEALER HAND -- SETTING CURSOR
        (x, y) = (Console.WindowWidth - Card.ASCII_WIDTH, yCoord_DealerLine - Card.ASCII_HEIGHT);
        Console.SetCursorPosition(x, y);
        // DEALER HAND -- DISPLAYING CARDS
        for (int i = 0; i < _dealer._hand._cards.Count; i++)
        {
            Utils.ASCII.DisplayASCII(_dealer._hand._cards[i].GetASCII(), false, backgroundColor: COLOR_B_DEALER);
            x -= Card.ASCII_WIDTH * (i + 1);
            Console.SetCursorPosition(x, y);
        }
        // RESET BACKGROUND COLOR
        Console.BackgroundColor = ASCII.DEFAULT_BACKGROUND;

        //
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
    }

    public void PrintAllHandsAsText()
    {
        // int cardPadding = 18;
        // int currentScore = 0;
        // Console.WriteLine($"\nUSER'S HAND:");
        // foreach (Card card in _user._hand._cards)
        // {
        //     Console.WriteLine(card.ToString().PadRight(cardPadding) + $"({card._value})");
        //     currentScore += card._value;
        // }
        // _user._hand._currentScore = currentScore;
        // Console.WriteLine($"|------------ SCORE: {_user._hand._currentScore} |");
        PrintSingleHandAsText(_user);

        Console.WriteLine("\nVS.");

        PrintSingleHandAsText(_dealer);

        // currentScore = 0;
        // Console.WriteLine("\nDEALER'S HAND");
        // foreach (Card card in _dealer._hand._cards)
        // {
        //     Console.WriteLine("|" + card.ToString().PadRight(cardPadding) + $"({card._value}) |");
        //     currentScore += card._value;
        // }
        // _dealer._hand._currentScore = currentScore;
        // Console.WriteLine($"|----------- SCORE: {_dealer._hand._currentScore} |");
    }
    public void PrintSingleHandAsText(Player player)
    {
        int cardPadding = 18;
        int sectionWidth = 24;

        string playerName = ((player._hand._isDealer) ? "DEALER" : "USER");
        int currentScore = 0;
        Console.WriteLine("\n" + $"{playerName}'S HAND:".PadRight(sectionWidth - 1, '-') + ".");
        foreach (Card card in player._hand._cards)
        {
            Console.WriteLine("| " + $"{card.ToString().PadRight(cardPadding) + card._value.ToString()}".PadRight(sectionWidth - 4) + " |");
            currentScore += card._value;
        }
        player._hand._currentScore = currentScore;
        Console.WriteLine($"|___________ SCORE: {player._hand._currentScore}".PadRight(sectionWidth-1) + "|");
        Console.WriteLine();
    }
}