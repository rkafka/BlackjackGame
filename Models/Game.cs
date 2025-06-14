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
    public void UpdateHands()
    {
        UI_Hands(); //(_user, _dealer);
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.WriteLine();
    }

    public void UI_Hands()//(User user, Dealer dealer)
    {
        int height = 10;
        int yCoord_UserLine = Console.WindowHeight - height;
        Console.SetCursorPosition(0, yCoord_UserLine);
        Console.Write("".PadRight(Console.WindowWidth, '_'));

        // Make sure the space representing the user's hand area is cleared
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Write("".PadRight(Console.WindowWidth * (height - 3), ' '));
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
        Console.SetCursorPosition(0, yCoord_DealerLine - (height - 3));
        Console.Write("".PadRight(Console.WindowWidth * (height - 3), ' '));
        Console.BackgroundColor = ASCII.DEFAULT_BACKGROUND;

        // DEALER TITLE
        int x = (Console.WindowWidth - (Utils.ASCII.ascii_Dealer.Split('\n')[0]).Length);
        int y = (yCoord_UserLine - Utils.ASCII.ascii_User.Split('\n').Length);
        Console.SetCursorPosition(x, y);
        Utils.ASCII.DisplayASCII(Utils.ASCII.ascii_Dealer, leftToRight:false, foregroundColor: COLOR_F_DEALER);
        // DEALER HAND -- SETTING CURSOR
        (x, y) = (Console.WindowWidth - Card.ASCII_WIDTH, yCoord_DealerLine - Card.ASCII_HEIGHT);
        Console.SetCursorPosition(x, y);
        // DEALER HAND -- DISPLAYING CARDS
        for (int i = 0; i < _dealer._hand._cards.Count; i++)
        {
            Utils.ASCII.DisplayASCII(_dealer._hand._cards[i].GetASCII(), false, backgroundColor: COLOR_B_DEALER);
            x -= Card.ASCII_WIDTH * (i+1);
            Console.SetCursorPosition(x, y);
        }
        // RESET BACKGROUND COLOR
        Console.BackgroundColor = ASCII.DEFAULT_BACKGROUND;

        //
        Console.SetCursorPosition(0, Console.WindowHeight-1);        
    }
}