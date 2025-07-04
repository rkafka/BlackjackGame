using System.Runtime.CompilerServices;
using BlackjackGame.Models;
using BlackjackGame.UI;

namespace BlackjackGame.Core;

/// <summary>
/// Main game engine for Blackjack. Manages game state, user, dealer, and UI.
/// </summary>
public class GameEngine
{
    private Deck _deck;
    private Dealer _dealer;
    private readonly IGameUI _ui;

    /// <summary>
    /// Gets or sets the user for the game.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Gets the current deck in use.
    /// </summary>
    public Deck Deck => _deck;

    /// <summary>
    /// Gets the dealer for the game.
    /// </summary>
    public Dealer Dealer => _dealer;

    /// <summary>
    /// Gets the UI interface for the game.
    /// </summary>
    public IGameUI UI => _ui;

    /// <summary>
    /// Constructor for the GameEngine that instantiates a Deck, User, and Dealer for the game to use
    /// throughout its execution. The UI object determines whether the Text-Based or ASCII Art display is used.
    /// </summary>
    /// <param name="ui">UI object passed in, determines the display type.</param>
    public GameEngine(IGameUI ui)
    {
        _ui = ui;
        _deck = new Deck(doShuffle: true, getCardsToOmit:GetAllInPlayCards);
        User = new(GameRules.UserStartingMoney);
        _dealer = new();
    }
    /// <summary>
    /// For testing: allows injection of custom deck, user, and dealer.
    /// </summary>
    public GameEngine(IGameUI ui, Deck customDeck, User user, Dealer dealer)
    {
        _ui = ui;
        _deck = customDeck;
        User = user;
        _dealer = dealer;
    }


    public List<Card> GetAllInPlayCards() { return [.. User.Hand.Cards, .. Dealer.Hand.Cards]; }

    /// <summary>
    /// Starts a new game round: displays the title, handles betting, deals initial cards, checks for blackjack,
    /// manages player and dealer turns, determines the winner, and resets cards for the next round.
    /// </summary>
    /// <returns>True if the user has enough money to continue playing, false otherwise.</returns>
    public bool PlayRound()
    {
        _ui.DisplayTitle();

        bool keepPlayingAfterWin = false; // TO-DO: add a toggle for this, modified by user input

        // INITIAL DRAW
        ChooseBet();
        InitialDraw();
        _ui.DisplayHands(User, Dealer, Dealer.DoHideFirstCard);

        // CHECK FOR BLACKJACK
        bool foundNatural = GameRules.CheckForBlackjack(_ui, User, Dealer, wouldBeNatural: true);
        if (foundNatural)
        {
            Dealer.DoHideFirstCard = false;
            _ui.DisplayHands(User, Dealer);
            _ui.PromptToContinue();
        }
        else
        {
            // PLAYER'S TURN
            PlayerTurnResult playerResult = PlayerTurn();
            _ui.PromptToContinue();
            if (playerResult != PlayerTurnResult.Bust && playerResult != PlayerTurnResult.Surrender)
            {
                // REVEAL DEALER'S HIDDEN CARD
                Dealer.DoHideFirstCard = false;
                _ui.RevealDealersHiddenCard(User, Dealer);
                // DEALER's TURN
                DealerTurn();
            }
            // DECIDE THE WINNER
            if(playerResult != PlayerTurnResult.Surrender)
                GameRules.DecideWinner(_ui, User, Dealer);
        }
        // If natural blackjack found, winner is already decided in GameRules

        // ResetCards(); // handled by deck
        ResetHands();

        Dealer.DoHideFirstCard = true;
        return (User.CurrentMoney >= GameRules.MinimumBet) && ((User.CurrentMoney < GameRules.GameWinConditionTarget) || (keepPlayingAfterWin));
    }

    /// <summary>
    /// Draws two cards to the user's hand and two cards to the dealer's hand at the start of a round.
    /// </summary>
    public void InitialDraw()
    {
        User.Hand.AddCard(_deck);
        User.Hand.AddCard(_deck);
        Dealer.Hand.AddCard(_deck);
        Dealer.Hand.AddCard(_deck);
    }

    /// <summary>
    /// Handles the player's turn, allowing them to choose actions (hit, stand, etc.) until they stand or bust.
    /// </summary>
    /// <returns>PlayerTurnResult indicating how the turn ended.</returns>
    private PlayerTurnResult PlayerTurn()
    {
        bool keepDrawing = true;
        bool busted = false;
        bool firstTurn = true;
        while (!GameRules.CheckHandForBlackjack(User.Hand) && keepDrawing)
        {
            // PLAYER OPTIONS: [1] Hit  [2] Stand  [3] Surrender [4] Double Down [5] Split
            if (!int.TryParse(_ui.PromptPlayerAction(firstTurn), out int playerChoice))
                continue;
            switch (playerChoice)
            {
                case 1:
                    // HIT
                    _ui.PlayerAction_ChoiceMessage("Hit");
                    keepDrawing = PlayerAction_Hit();
                    busted = GameRules.CheckForBust(User.Hand);
                    if (busted)
                    {
                        Console.WriteLine("You BUST. That's a shame...");
                        return PlayerTurnResult.Bust;
                    }
                    break;
                case 2:
                    // STAND
                    _ui.PlayerAction_ChoiceMessage("Stand");
                    return PlayerTurnResult.Stand;
                case 3:
                    // SURRENDER
                    if (!firstTurn)
                    {
                        Console.WriteLine("ERROR: can only surrender as your first action after the initial deal");
                        continue;
                    }
                    _ui.PlayerAction_ChoiceMessage("Surrender");
                    PlayerAction_Surrender();
                    return PlayerTurnResult.Surrender;
                case 4:
                    // DOUBLE DOWN
                    _ui.PlayerAction_ChoiceMessage("Double Down");
                    if (!firstTurn)
                    {
                        Console.WriteLine("ERROR: can only double down as your first action after the initial deal");
                        continue;
                    }
                    else
                    {
                        keepDrawing = PlayerAction_DoubleDown();
                        busted = GameRules.CheckForBust(User.Hand);
                        if (busted)
                        {
                            Console.WriteLine("You BUST. That's a shame...");
                            return PlayerTurnResult.Bust;
                        }
                        return PlayerTurnResult.DoubleDown;
                    }
                case 5:
                    // SPLIT
                    if (!firstTurn)
                    {
                        Console.WriteLine("ERROR: can only split as your first action after the initial deal");
                        continue;
                    }
                    _ui.PlayerAction_ChoiceMessage("Split");
                    _ui.PlayerAction_NotSupportedMessage();
                    return PlayerTurnResult.Split;
                default:
                    Console.WriteLine("Your input was not an integer in the correct range (1-5). Please try again.");
                    break;
            }
            firstTurn = false;
        }
        Console.WriteLine();
        // If we exit the loop without busting, surrendering, or standing, treat as stand
        return PlayerTurnResult.Stand;
    }

    /// <summary>
    /// Handles the logic for the player choosing to hit: adds a card to the user's hand, displays the result,
    /// and checks for blackjack or bust.
    /// </summary>
    /// <returns>True if the player can continue (not blackjack or bust), false otherwise.</returns>
    public bool PlayerAction_Hit()
    {
        User.Hand.AddCard(_deck);
        _ui.CardDrawnMessage(User);
        _ui.DisplayHands(User, Dealer, hideDealersFirstCard: true);

        bool gotBlackjack = GameRules.CheckHandForBlackjack(User.Hand);
        if (gotBlackjack)
            Console.WriteLine("You got blackjack! Ending your turn.");

        bool busted = GameRules.CheckForBust(User.Hand);
        bool canKeepDrawing = (!gotBlackjack && !busted);
        return canKeepDrawing;
    }

    /// <summary> Handles the logic for the player choosing to double down. (TO-DO: complete implementation) </summary>
    /// <returns>True if the player can continue, false otherwise.</returns>
    public bool PlayerAction_DoubleDown()
    {
        int oldBet = User.Hand.BetAmount;
        bool betRaiseSuccessful = RaiseBet(oldBet);
        Console.Write($"Your bet was doubled from {oldBet:C0} to {User.Hand.BetAmount:C2}, now drawing a card before ending your turn.");
        if (!betRaiseSuccessful)
            _ui.PromptAfterError($"You don't have enough money to double your bet, you have {User.CurrentMoney} and need {User.Hand.BetAmount * 2}", isBet: false, tryAgain: false);
        else
            PlayerAction_Hit();
        return !betRaiseSuccessful; // TO-DO: finish
    }

    /// <summary> </summary>
    public void PlayerAction_Surrender()
    {
        // TO-DO: warning message if game cannot continue should you surrender (difficulty dependent?)
        float surrenderReturn = User.Hand.BetAmount * GameRules.SurrenderReturnRatio;
        User.CurrentMoney += surrenderReturn;
        Console.WriteLine($"The round is over, half of your {User.Hand.BetAmount:C2} bet has been returned ({surrenderReturn:C2}).");
        User.WinningsRecord.Add(-1 * (User.Hand.BetAmount - surrenderReturn));
        User.NumLosses++;
    }


    /// <summary>
    /// Handles the dealer's turn: dealer draws cards until reaching at least 17 or busting, displaying each action.
    /// </summary>
    /// <returns>True if the dealer does not bust, false if the dealer busts.</returns>
    private bool DealerTurn()
    {
        while (Dealer.Hand.CurrentScore < 17)
        {
            Console.Clear();
            // Draw a card from the deck and add it to the Dealer's hand
            Dealer.Hand.AddCard(_deck);
            // Retrieve the card just added & display a message describing this action
            _ui.CardDrawnMessage(Dealer);
            // Update the display representing players' hands
            _ui.DisplayHands(User, Dealer, hideDealersFirstCard: Dealer.DoHideFirstCard);
            _ui.PromptToContinue();
        }

        // Check for bust
        if (Dealer.Hand.CurrentScore > 21)
        {
            Console.WriteLine("The Dealer BUST! That means ...");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Resets the deck and both hands for a new round, creating a new shuffled deck and empty hands.
    /// </summary>
    public void ResetHands()
    {
        // _deck = new Deck(doShuffle: true);  // NOW HANDLED BY DECK CLASS WHEN DECK IS EMPTY
        User.Hand = new Hand(betAmount: -1, isDealer: false);
        Dealer.Hand = new Hand(betAmount: -1, isDealer: true);
    }

    /// <summary>
    /// Prompts the user to choose a bet amount and validates the input, ensuring it is within allowed limits.
    /// </summary>
    public void ChooseBet()
    {
        _ui.PromptForBet(User);

        bool validBet = false;
        while (!validBet)
        {
            if (!int.TryParse(Console.ReadLine(), out int betAmount))
                _ui.PromptAfterError("Your bet must be an integer", isBet: true);
            else
            {
                if (betAmount > User.CurrentMoney)
                    _ui.PromptAfterError("Your bet must be less than your starting money", isBet: true);
                else if (betAmount < GameRules.MinimumBet)
                    _ui.PromptAfterError($"The minimum bet is {GameRules.MinimumBet:C0}", isBet: true);
                else 
                    validBet = SetBet(betAmount); // SUCCESS!
            }
        }
    }

    /// <summary> Sets the user's bet for the round, deducting the bet from their current money.
    /// </summary>
    /// <param name="newBet">The new bet amount to set.</param>
    /// <returns>True if the bet was set successfully, false otherwise.</returns>
    public bool SetBet(int newBet)
    {
        if (newBet > User.CurrentMoney)
            return false;
        else
        {
            // set the money aside as part of the hand
            User.CurrentMoney -= newBet;
            User.Hand.BetAmount = newBet;
            return true;
        }
    }

    /// <summary> Raises the user's bet by a specified amount, adding it to the existing bet amount, 
    /// if they have enough money. </summary>
    /// <param name="raise">The amount to increase the bet by.</param>
    /// <returns>True if the bet was incremented successfully, false otherwise.</returns>
    public bool RaiseBet(int raise)
    {
        if (raise > User.CurrentMoney)
            return false;
        else
        {
            this.User.CurrentMoney -= raise;
            this.User.Hand.BetAmount += raise;
            return true;
        }
    }
}

/// <summary>
/// Enum representing the possible end states of the player's turn. 
/// Includes 'Stand', 'Bust', 'Surrender', 'DoubleDown', and 'Split'
/// </summary>
/// <example><code>var f = PlayerTurnResult.Stand</code></example>
public enum PlayerTurnResult
{
    Stand,
    Bust,
    Surrender,
    DoubleDown,
    Split
}