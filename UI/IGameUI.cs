using BlackjackGame.Models;
using BlackjackGame.Core;

namespace BlackjackGame.UI;


public interface IGameUI
{
    /* -- CONSTANTS - */
    public const ConsoleColor COLOR_PROMPT = ConsoleColor.Yellow;
    public const ConsoleColor COLOR_SPECIAL_1 = ConsoleColor.Cyan;
    public const ConsoleColor COLOR_DEFAULT_BACKGROUND = ConsoleColor.Black;
    public const ConsoleColor COLOR_DEFAULT_FOREGROUND = ConsoleColor.White;
    public const ConsoleColor COLOR_GOOD = ConsoleColor.Green;
    public const ConsoleColor COLOR_NEUTRAL = ConsoleColor.Yellow;
    public const ConsoleColor COLOR_BAD = ConsoleColor.Red;


    /* - FUNCTIONS - */

    // SHOW TITLE
    public void DisplayTitle();
    // DISPLAY HAND(S) 
    public void DisplayHands(User user, Dealer dealer, bool hideDealersFirstCard);  // both/all hands
    public void DisplayHands(User user, Dealer dealer);  // overload: uses dealer.DoHideFirstCard
    public void DisplayHand(Player player, bool hideDealersFirstCard); // single hand

    // LAUNCH SCREEN
    public void LaunchScreen(bool waitForInput = true);

    // PLAYER ACTION MESSAGES
    public void PlayerAction_ChoiceMessage(string playerAction);
    public void PlayerAction_NotSupportedMessage();

    // PROMPTS (waits for user input)
    public string PromptPlayerAction(bool isFirstTurn = true, bool canDoubleDown = true);
    public void PromptToContinue();
    public void PromptForBet(User user);
    public void PromptAfterError(string problem, bool isBet = false, bool tryAgain = true);

    // ROUND RESULT MESSAGES
    public void ResultMessage_Win(User user, bool isNatural = false);
    public void ResultMessage_Tie(User user);
    public void ResultMessage_Loss(User user, bool isNatural = false);
    //
    public void RevealDealersHiddenCard(User user, Dealer dealer);
    //
    public void CardDrawnMessage(Player player);
    //
    public void GameOverMessage(User user);

    public void VictoryMessage(User user);

}