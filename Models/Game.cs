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
        return playerChoiceNum; // TO-DO: URGENT: update return code from playerActions
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



    public void PrintAllHandsAsText(bool hideDealersFirstCard = true)
    {
        // Print user's hand of cards, with scores included
        PrintSingleHandAsText(_user);

        Console.WriteLine("\nVS.");

        // Print dealer's hand of cards, with scores included
        PrintSingleHandAsText(_dealer, hideFirstCard: hideDealersFirstCard);

        Console.WriteLine();
    }
    public void PrintSingleHandAsText(Player player, bool hideFirstCard = false)
    {
        int cardPadding = 18;
        int sectionWidth = 24;

        string playerName = ((player._hand._isDealer) ? "DEALER" : "USER");
        int currentScore = 0;
        Console.WriteLine("\n" + $"{playerName}'S HAND:".PadRight(sectionWidth - 1, '-') + ".");
        for (int i = 0; i < player._hand._cards.Count; i++) // foreach(Card card in player._hand._cards)
        {
            var card = player._hand._cards[i];
            if (hideFirstCard && i == 0)
                Console.WriteLine($"| [HIDDEN]          ?? |");
            else
            {
                Console.WriteLine("| " + $"{card.ToString().PadRight(cardPadding) + card._value.ToString()}".PadRight(sectionWidth - 4) + " |");
                currentScore += card._value;
            }
        }
        if (hideFirstCard)
            Console.WriteLine($"|________ SCORE: {currentScore}".PadRight(sectionWidth - 5) + "+ ? |");
        else
            Console.WriteLine($"|___________ SCORE: {player._hand._currentScore}".PadRight(sectionWidth - 1) + "|");
    }

    public void DealersTurn()
    {
        /*
        1. Reveal Hidden Card
            After all players have completed their turns, the dealer flips over their facedown card to reveal the full hand.
        2. Must Hit Until 17 or More
            - The dealer must continue drawing cards (hit) until their hand totals 17 or more.
                - This includes “soft 17” (e.g., Ace + 6 = 17), unless the rules require hitting on soft 17.
        3. Must Stand on 17 or Higher
            - If the dealer has 17, 18, 19, 20, or 21, they must stand—no exceptions.
            - Some casinos require dealers to hit soft 17 (varies by house rules).
        4. Bust if Over 21
            - If the dealer's hand goes over 21, they bust. All remaining players win.
        */

        // Reveal Hidden Card
        PrintAllHandsAsText(hideDealersFirstCard: false);
        Thread.Sleep(5000);

        while (_dealer._hand._currentScore < 17)
        {
            Console.Clear();
            _user._hand.AddCard(_deck);
            Console.WriteLine($"\nThe dealer drew the {_user._hand._cards.Last()} (value: {_user._hand._cards.Last()._value})\n");
            Thread.Sleep(500);
            PrintAllHandsAsText(hideDealersFirstCard:false);
            Thread.Sleep(2000);
        }


    }

    public void DecideWinner()
    {
        Console.WriteLine("\n\n'DecideWinner()' is not implemented yet.\nPress enter to continue...");
        Console.ReadLine();
    }
}