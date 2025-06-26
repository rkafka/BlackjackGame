using BlackjackGame.Models;
using BlackjackGame.Core;

namespace BlackjackGame.UI;

public class UI_ASCII : IGameUI
{
    /*
        > CONSTANTS
    */
    // Colors
    public const ConsoleColor COLOR_F_USER = ConsoleColor.Blue;
    public const ConsoleColor COLOR_B_USER = ConsoleColor.DarkBlue;
    public const ConsoleColor COLOR_F_DEALER = ConsoleColor.Green;
    public const ConsoleColor COLOR_B_DEALER = ConsoleColor.DarkGreen;
    // Values




    public void CardDrawnMessage(Player player)
    {
        throw new NotImplementedException();
    }

    public void DisplayTitle()
    {
        throw new NotImplementedException();
    }

    public void DisplayHand(Player player, bool doHideFirstCard = true)
    {
        throw new NotImplementedException();
    }

    // DEV NOTE: formerly UI_Hands() in old version
    public void DisplayHands(User user, Dealer dealer, bool doHideFirstCard = true)
    {
        int distanceFromBottom = 10;
        int yCoord_UserLine = Console.WindowHeight - distanceFromBottom;
        Console.SetCursorPosition(0, yCoord_UserLine);
        Console.Write("".PadRight(Console.WindowWidth, '_'));

        // Make sure the space representing the user's hand area is cleared
        Console.BackgroundColor = COLOR_B_USER;
        Console.Write("".PadRight(Console.WindowWidth * (distanceFromBottom - 3), ' '));
        Console.BackgroundColor = Utils.ASCII.DEFAULT_BACKGROUND;

        // USER TITLE
        Console.SetCursorPosition(0, yCoord_UserLine - Utils.ASCII.ascii_User.Split('\n').Length);
        Utils.ASCII.DisplayASCII(Utils.ASCII.ascii_User, leftToRight: true, foregroundColor: COLOR_F_USER);
        // USER HAND
        Console.SetCursorPosition(0, yCoord_UserLine + 1);
        for (int i = 0; i < user.Hand.Cards.Count; i++)
        {
            Utils.ASCII.DisplayASCII(user.Hand.Cards[i].GetASCII(), true, backgroundColor: COLOR_B_USER);
        }
        // RESET BACKGROUND COLOR
        Console.BackgroundColor = Utils.ASCII.DEFAULT_BACKGROUND;



        int yCoord_DealerLine = yCoord_UserLine - Utils.ASCII.ascii_User.Split('\n').Length - 1;
        Console.SetCursorPosition(0, yCoord_DealerLine);

        Console.Write("".PadRight(Console.WindowWidth, '_'));
        // Make sure the space representing the dealer's hand area is cleared
        Console.BackgroundColor = COLOR_B_DEALER;
        Console.SetCursorPosition(0, yCoord_DealerLine - (distanceFromBottom - 3));
        Console.Write("".PadRight(Console.WindowWidth * (distanceFromBottom - 3), ' '));
        Console.BackgroundColor = Utils.ASCII.DEFAULT_BACKGROUND;

        // DEALER TITLE
        int x = (Console.WindowWidth - (Utils.ASCII.ascii_Dealer.Split('\n')[0]).Length);
        int y = (yCoord_UserLine - Utils.ASCII.ascii_User.Split('\n').Length);
        Console.SetCursorPosition(x, y);
        Utils.ASCII.DisplayASCII(Utils.ASCII.ascii_Dealer, leftToRight: false, foregroundColor: COLOR_F_DEALER);
        // DEALER HAND -- SETTING CURSOR
        (x, y) = (Console.WindowWidth - Card.ASCII_WIDTH, yCoord_DealerLine - Card.ASCII_HEIGHT);
        Console.SetCursorPosition(x, y);
        // DEALER HAND -- DISPLAYING CARDS
        for (int i = 0; i < dealer.Hand.Cards.Count; i++)
        {
            Utils.ASCII.DisplayASCII(dealer.Hand.Cards[i].GetASCII(), false, backgroundColor: COLOR_B_DEALER);
            x -= Card.ASCII_WIDTH * (i + 1);
            Console.SetCursorPosition(x, y);
        }
        // RESET BACKGROUND COLOR
        Console.BackgroundColor = Utils.ASCII.DEFAULT_BACKGROUND;

        //
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
    }


    public void GameOverMessage(GameEngine engine)
    {
        throw new NotImplementedException();
    }


    public void LaunchScreen(bool waitForInput = true)
    {
        throw new NotImplementedException();
    }


    public string PromptPlayerAction(bool isFirstTurn = true)
    {
        throw new NotImplementedException();
    }
    public void PromptAfterError(string problem, bool isBet = false, bool tryAgain=false)
    {
        throw new NotImplementedException();
    }

    public void PromptToContinue()
    {
        throw new NotImplementedException();
    }

    public void PromptForBet(User user)
    {
        throw new NotImplementedException();
    }

    public void PlayerAction_ChoiceMessage(string playerAction)
    {
        throw new NotImplementedException();
    }
    public void PlayerAction_NotSupportedMessage()
    {
        throw new NotImplementedException();
    }
    public void PlayerAction_HitMessage(User user)
    {
        throw new NotImplementedException();
    }

    public void ResetConsoleColors()
    {
        Console.BackgroundColor = Utils.ASCII.DEFAULT_BACKGROUND;
        Console.ForegroundColor = Utils.ASCII.DEFAULT_FOREGROUND;
    }

    public void ResultMessage_Win(User user, bool isNatural = false)
    {
        throw new NotImplementedException();
    }
    public void ResultMessage_Tie(User user)
    {
        throw new NotImplementedException();
    }
    public void ResultMessage_Loss(User user)
    {
        throw new NotImplementedException();
    }

    public void RevealDealersHiddenCard(User user, Dealer dealer) {
        throw new NotImplementedException();
    }
}