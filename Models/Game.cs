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


    //
    public const int SCORE_BLACKJACK = 21;
    public const int SCORE_SOFT_BLACKJACK = 17;
    //
    public const int WIN_RATIO = 2;
    public const int LOSS_RATIO = 1;

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

    // public void InitialDraw()
    // {
    //     _user._hand.AddCard(_deck);
    //     _user._hand.AddCard(_deck);
    //     _dealer._hand.AddCard(_deck);
    //     _dealer._hand.AddCard(_deck);
    // }

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
                    if (_user._hand._currentScore == 21)
                        break;
                    else if (_user._hand._currentScore > 21)
                    {
                        // while(_user._hand._currentScore > 21) // TO-DO: check for aces to revalue
                        keepDrawing = false;
                        Console.WriteLine("BUST. Your cards cumulative value is greater than 21.");
                        return 0;
                    }
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



    // public void PrintAllHandsAsText(bool hideDealersFirstCard = true)
    // {
    //     // Print user's hand of cards, with scores included
    //     PrintSingleHandAsText(_user);

    //     Console.WriteLine("\nVS.");

    //     // Print dealer's hand of cards, with scores included
    //     PrintSingleHandAsText(_dealer, hideFirstCard: hideDealersFirstCard);

    //     Console.WriteLine();
    // }

    // public static void PrintSingleHandAsText(Player player, bool hideFirstCard = false)
    // {
    //     int cardPadding = 18;
    //     int sectionWidth = 24;

    //     string playerName = ((player._hand._isDealer) ? "DEALER" : "USER");
    //     int currentScore = 0;
    //     Console.WriteLine("\n" + $"{playerName}'S HAND:".PadRight(sectionWidth - 1, '-') + ".");
    //     for (int i = 0; i < player._hand._cards.Count; i++) // foreach(Card card in player._hand._cards)
    //     {
    //         Card card = player._hand._cards[i];
    //         // hide first card if its player's turn
    //         if (hideFirstCard && i == 0) {
    //             Console.WriteLine("| [");
    //             Console.BackgroundColor = ConsoleColor.DarkGray;
    //             Console.Write("HIDDEN");
    //             Console.ResetColor();
    //             Console.Write("]          ?? |");
    //         }
    //         else // output card info to hand display
    //         {
    //             Console.WriteLine("| " + $"{card.ToString().PadRight(cardPadding) + Card.GetValue(card._rank).ToString()}".PadRight(sectionWidth - 4) + " |");
    //             currentScore += card._value;
    //         }
    //     }
    //     if (hideFirstCard)
    //         Console.WriteLine($"|________ SCORE: {currentScore}".PadRight(sectionWidth - 5) + "+ ? |");
    //     else
    //     {
    //         string scoreLineStart = "|___________ SCORE: ";
    //         Console.Write(scoreLineStart);
    //         if (player._hand._currentScore > 21)
    //             Console.ForegroundColor = ConsoleColor.Red;
    //         else if (player._hand._currentScore == 21)
    //             Console.ForegroundColor = ConsoleColor.Yellow;
    //         Console.Write(player._hand._currentScore.ToString().PadRight(sectionWidth - 1 - scoreLineStart.Length));
    //         Console.ResetColor();
    //         Console.WriteLine("|");
    //     }
    // }

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
    // public void DealersTurn()
    // {
    //     // Reveal Hidden Card
    //     PrintAllHandsAsText(hideDealersFirstCard: false);
    //     Thread.Sleep(5000);

    //     while (_dealer._hand._currentScore < 17)
    //     {
    //         Console.Clear();
    //         _dealer._hand.AddCard(_deck);
    //         Console.WriteLine($"\nThe dealer drew the {_dealer._hand._cards.Last()} (value: {_dealer._hand._cards.Last()._value})\n");
    //         Thread.Sleep(500);
    //         PrintAllHandsAsText(hideDealersFirstCard: false);
    //         Thread.Sleep(2000);
    //     }

    //     // Check for bust
    //     if (_dealer._hand._currentScore > 21)
    //     {
    //         Console.WriteLine("The Dealer BUST! That means ...");
    //     }


    // }

    public void DecideWinner()
    {
        bool userBusted = (_user._hand._currentScore > 21);
        bool dealerBusted = (_dealer._hand._currentScore > 21);

        // for(int i = 0; i < _user._hands.Count; i++)
        if (userBusted || (_user._hand._currentScore < _dealer._hand._currentScore && !dealerBusted)) // DEALER wins (USER loses)
        {
            resultLose();
        }
        else if (dealerBusted || _user._hand._currentScore > _dealer._hand._currentScore) // USER wins!!
        {
            resultWin();
        }
        else // TIED
        {
            resultTie();
        }

        Console.WriteLine("\nPress enter to continue...");
        Console.ReadLine();
    }

    // public void resultLose()
    // {
    //     _user._currentMoney -= _user._hand._betAmount;
    //     _user._numLosses++;
    //     Console.WriteLine($"You lost... Your bet of ${_user._hand._betAmount} has been deducted from your money.");
    //     // _user._hand._betAmount = 0;
    //     Console.Write($"Remaining Money:  {_user._currentMoney:C0}  |  W/L/T Record:  ");
    //     _user.PrintRecord_Colored(doNewLine: true);
    // }
    // public void resultWin()
    // {
    //     _user._currentMoney += _user._hand._betAmount * WIN_RATIO;
    //     _user._numWins++;
    //     Console.WriteLine($"You won! Your bet of ${_user._hand._betAmount} has been doubled and returned to you.");
    //     // _user._hand._betAmount = 0;
    //     Console.Write($"Remaining Money:  {_user._currentMoney:C0}  |  W/L/T Record:  ");
    //     _user.PrintRecord_Colored(doNewLine: true);
    // }
    // public void resultTie()
    // {
    //     _user._numTies++;
    //     Console.WriteLine($"You tied. Your bet of ${_user._hand._betAmount} has been returned to you.");
    //     // _user._hand._betAmount = 0;
    //     Console.Write($"Remaining Money:  {_user._currentMoney:C0}  |  W/L/T Record:  ");
    //     _user.PrintRecord_Colored(doNewLine: true);
    // }

    public void SetBet()
    {
        Console.WriteLine("Starting new round...");

        Console.WriteLine($"You currently have {_user._currentMoney:C0} to gamble with.");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("How much would you like to bet? (must be an integer greater than 0) -->");
        Console.ResetColor();
        Console.Write(" $");

        // bool waitForBet = true;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int betAmount))
            {
                if (betAmount > _user._currentMoney)
                {
                    _ui.RepromptBet(problem: "Your bet must be less than your starting money");
                }
                else if (betAmount <= 0)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("Oops! Your bet must be greater than zero, try again:");
                    Console.ResetColor();
                    Console.Write(" $");
                }
                else
                {
                    _user._hand._betAmount = betAmount;
                    return;
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("Oops! Your bet must be an integer, try again: ");
                Console.ResetColor();
                Console.Write(" $");
            }
        }

    }

    // public void ResetCards()
    // {
    //     _deck = new Deck(doShuffle: true);
    //     _user._hand = new Hand(betAmount: -1, isDealer: false);
    //     _dealer._hand = new Hand(betAmount: 0, isDealer: true);
    // }

    // public void GameOver()
    // {
    //     string gameOverTitle = "[ GAME OVER ]";
    //     // DebugTools.PrintSectionHeader(gameOverTitle, false);
    //     Console.WriteLine("".PadRight(Console.WindowWidth/2,'-') + gameOverTitle + "".PadRight(Console.WindowWidth/2,'-'));
    //     Console.Write("Your final record was ");
    //     _user.PrintRecord_Colored(doNewLine:true);
    // }
}
