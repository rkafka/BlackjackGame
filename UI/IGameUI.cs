using BlackjackGame.Models;
using BlackjackGame.Core;

namespace BlackjackGame.UI;


public interface IGameUI
{
    public const ConsoleColor COLOR_PROMPT = ConsoleColor.Yellow;
    public const ConsoleColor COLOR_DEFAULT_BACKGROUND = ConsoleColor.Black;
    public const ConsoleColor COLOR_DEFAULT_FOREGROUND = ConsoleColor.White;
    public const ConsoleColor COLOR_GOOD = ConsoleColor.Green;
    public const ConsoleColor COLOR_BAD = ConsoleColor.Red;

    public void DisplayTitle();

    public void DisplayHands(User user, Dealer dealer, bool hideDealersFirstCard);  // both/all hands
    public void DisplayHand(Player player, bool hideDealersFirstCard); // single hand

    public void LaunchScreen(bool waitForInput = true);
    
    public void PlayerAction_ChoiceMessage(string playerAction);
    public void PlayerAction_NotSupportedMessage();

    /// <summary> Prompts the player to select a Player Action (Hit, Stand, Double Down, etc.) by typing their associated number. </summary>
    /// <returns>String containing the input read in from the user</returns>
    public string PromptPlayerAction(bool isFirstTurn = true);
    public void PromptToContinue();
    public void PromptForBet(User user);
    public void PromptAfterError(string problem, bool isBet = false, bool tryAgain = true);

    public void ResetConsoleColors();

    public void ResultMessage_Win(User user, bool isNatural = false);
    public void ResultMessage_Tie(User user);
    public void ResultMessage_Loss(User user);

    public void RevealDealersHiddenCard(User user, Dealer dealer);

    public void CardDrawnMessage(Player player);

    public void GameOverMessage(GameEngine engine);

}