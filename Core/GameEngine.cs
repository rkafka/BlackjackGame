using System.Runtime.CompilerServices;
using BlackjackGame.Models;
using BlackjackGame.UI;

namespace BlackjackGame.Core;

public class GameEngine
{
    private Deck _deck;
    public User _user;
    private Dealer _dealer;
    private readonly IGameUI _ui;

    /// <summary> 
    /// Constructor for the GameEngine that instantiates a Deck, User, and Dealer for the game to use
    /// throughout its execution. The UI object determines whether the Text-Based or ASCII Art display is used.
    /// </summary>
    /// <param name="ui">UI object passed in, determines the display type.</param>
    public GameEngine(IGameUI ui)
    {
        this._ui = ui;

        this._deck = new Deck(doShuffle: true);
        this._user = new();
        this._dealer = new();
    }

    /// <summary>
    /// Starts a new game round: displays the title, handles betting, deals initial cards, checks for blackjack,
    /// manages player and dealer turns, determines the winner, and resets cards for the next round.
    /// </summary>
    /// <returns>True if the user has enough money to continue playing, false otherwise.</returns>
    public bool StartGame()
    {
        _ui.DisplayTitle();

        // INITIAL DRAW
        ChooseBet();
        InitialDraw();
        _ui.DisplayHands(_user, _dealer, _dealer._doHideFirstCard);

        // CHECK FOR BLACKJACK
        bool foundNatural = GameRules.CheckForBlackjack(_ui, _user, _dealer, wouldBeNatural: true);
        if (!foundNatural)
        {
            // PLAYER'S TURN
            // -- Allow to chose between available options with text input (prompted)
            bool playerBusted = PlayerTurn();
            _ui.PromptToContinue();
            if (!playerBusted)
            {
                // REVEAL DEALER'S HIDDEN CARD
                _dealer._doHideFirstCard = false;
                _ui.RevealDealersHiddenCard(_user, _dealer);
                // DEALER's TURN
                DealerTurn();
            }
            // DECIDE THE WINNER
            GameRules.DecideWinner(_ui, _user, _dealer);
        }

        // If natural blackjack found, winner is already decided in GameRules

        ResetCards();
        return (_user._currentMoney > GameRules.MINIMUM_BET);
    }

    /// <summary> Draws two cards to the user's hand and two cards to the dealer's hand at the start of a round. </summary>
    public void InitialDraw()
    {
        _user._hand.AddCard(_deck);
        _user._hand.AddCard(_deck);
        _dealer._hand.AddCard(_deck);
        _dealer._hand.AddCard(_deck);
    }

    /// <summary>
    /// Handles the player's turn, allowing them to choose actions (hit, stand, etc.) until they stand or bust.
    /// </summary>
    /// <returns>True if the player busts, false otherwise.</returns>
    private bool PlayerTurn()
    {
        bool keepDrawing = true;
        bool busted = false;
        while (!GameRules.CheckForBlackjack(_user._hand) && keepDrawing)
        {
            // PLAYER OPTIONS: [1] Hit  [2] Stand  [3] Double Down   |   Type the number to select:     TO-DO: [3] Double Down [4] Split, [5] Surrender
            if (!int.TryParse(_ui.PromptPlayerAction(), out int playerChoice))
                continue;
            switch (playerChoice)
            {
                case 1:
                    // HIT
                    _ui.PlayerAction_ChoiceMessage("Hit");
                    keepDrawing = PlayerAction_Hit();
                    busted = GameRules.CheckForBust(_user._hand);
                    if (busted)
                        Console.WriteLine("You BUST. That's a shame...");
                    break;
                case 2:
                    // STAND
                    _ui.PlayerAction_ChoiceMessage("Stand");
                    keepDrawing = false;
                    break;
                case 3:
                    // DOUBLE DOWN
                    _ui.PlayerAction_ChoiceMessage("Double Down");
                    keepDrawing = PlayerAction_DoubleDown();
                    busted = GameRules.CheckForBust(_user._hand);
                    if (busted)
                        Console.WriteLine("You BUST. That's a shame...");
                    break;
                case 4:
                    // SPLIT
                    _ui.PlayerAction_ChoiceMessage("Split");
                    _ui.PlayerAction_NotSupportedMessage();
                    break;
                case 5:
                    // SURRENDER
                    _ui.PlayerAction_ChoiceMessage("Surrender");
                    _ui.PlayerAction_NotSupportedMessage();
                    break;
                default:
                    Console.WriteLine("Your input was not an integer in the correct range (1-5). Please try again.");
                    break;
            }
        }
        Console.WriteLine();
        return busted;
    }

    /// <summary>
    /// Handles the logic for the player choosing to hit: adds a card to the user's hand, displays the result,
    /// and checks for blackjack or bust.
    /// </summary>
    /// <returns>True if the player can continue (not blackjack or bust), false otherwise.</returns>
    public bool PlayerAction_Hit()
    {
        _user._hand.AddCard(_deck);
        _ui.CardDrawnMessage(_user);
        _ui.DisplayHands(_user, _dealer, hideDealersFirstCard: true);
        return (!GameRules.CheckForBlackjack(_user._hand) && !GameRules.CheckForBust(_user._hand));
    }

    /// <summary> Handles the logic for the player choosing to double down. (TO-DO: complete implementation) </summary>
    /// <returns>True if the player can continue, false otherwise.</returns>
    public bool PlayerAction_DoubleDown()
    {
        int oldBet = _user._hand._betAmount;
        bool betRaiseSuccessful = RaiseBet(oldBet);
        Console.Write($"Your bet was doubled from {oldBet:C0} to {_user._hand._betAmount:C2}, now drawing a card before ending your turn.");
        if (!betRaiseSuccessful)
            _ui.PromptAfterError($"You don't have enough money to double your bet, you have {_user._currentMoney} and need {_user._hand._betAmount * 2}", isBet: false, tryAgain: false);
        else
            PlayerAction_Hit();
        return !betRaiseSuccessful; // TO-DO: finish
    }

    /// <summary> Handles the dealer's turn: dealer draws cards until reaching at least 17 or busting, displaying each action.
    /// </summary>
    /// <returns>True if the dealer does not bust, false if the dealer busts.</returns>
    private bool DealerTurn()
    {
        while (_dealer._hand._currentScore < 17)
        {
            Console.Clear();
            // Draw a card from the deck and add it to the Dealer's hand
            _dealer._hand.AddCard(_deck);
            // Retrieve the card just added & display a message describing this action
            _ui.CardDrawnMessage(_dealer);
            // Update the display representing players' hands
            _ui.DisplayHands(_user, _dealer, hideDealersFirstCard: _dealer._doHideFirstCard);
            _ui.PromptToContinue();
        }

        // Check for bust
        if (_dealer._hand._currentScore > 21)
        {
            Console.WriteLine("The Dealer BUST! That means ...");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Resets the deck and both hands for a new round, creating a new shuffled deck and empty hands.
    /// </summary>
    public void ResetCards()
    {
        _deck = new Deck(doShuffle: true);
        _user._hand = new Hand(betAmount: -1, isDealer: false);
        _dealer._hand = new Hand(betAmount: -1, isDealer: true);
    }

    /// <summary>
    /// Prompts the user to choose a bet amount and validates the input, ensuring it is within allowed limits.
    /// </summary>
    public void ChooseBet()
    {
        _ui.PromptForBet(_user);

        bool validBet = false;
        while (!validBet)
        {
            if (!int.TryParse(Console.ReadLine(), out int betAmount))
                _ui.PromptAfterError("Your bet must be an integer", isBet: true);
            else
            {
                if (betAmount > _user._currentMoney)
                    _ui.PromptAfterError("Your bet must be less than your starting money", isBet: true);
                else if (betAmount < GameRules.MINIMUM_BET)
                    _ui.PromptAfterError($"The minimum bet is {GameRules.MINIMUM_BET:C0}", isBet: true);
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
        if (newBet > _user._currentMoney)
            return false;
        else
        {
            // set the money aside as part of the hand
            _user._currentMoney -= newBet;
            _user._hand._betAmount = newBet;
            return true;
        }
    }

    /// <summary> Raises the user's bet by a specified amount, adding it to the existing bet amount, 
    /// if they have enough money. </summary>
    /// <param name="raise">The amount to increase the bet by.</param>
    /// <returns>True if the bet was incremented successfully, false otherwise.</returns>
    public bool RaiseBet(int raise)
    {
        if (_user._hand._betAmount + raise > _user._currentMoney)
            return false;
        else
        {
            _user._currentMoney -= raise;
            _user._hand._betAmount += raise;
            return true;
        }
    }
}