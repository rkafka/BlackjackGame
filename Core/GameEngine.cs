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

    public GameEngine(IGameUI ui)
    {
        this._ui = ui;

        this._deck = new Deck(doShuffle: true);
        this._user = new();
        this._dealer = new();
    }

    public bool StartGame()
    {
        _ui.DisplayTitle();

        // INITIAL DRAW
        ChooseBet();
        InitialDraw();
        _ui.DisplayHands(_user, _dealer, _dealer._doHideFirstCard);

        // CHECK FOR BLACKJACK
        /*
        // --(IF FOUND)--> modify money based on win/loss and restart 
        // bool hasBlackjack_User = GameRules.CheckForBlackjack(_user._hand);
        // bool hasBlackjack_Dealer = GameRules.CheckForBlackjack(_dealer._hand);
        // if (hasBlackjack_User || hasBlackjack_Dealer)
        // {
        //     if (hasBlackjack_User && hasBlackjack_Dealer)
        //         GameRules.ResultTie(_ui, _user);
        //     else if (hasBlackjack_User)
        //         GameRules.ResultWin(_ui, _user);
        //     else // hasBlackjack_dealer
        //         GameRules.ResultLose(_ui, _user);
        // }
        // else */
        if (!GameRules.CheckForBlackjack(_ui, _user, _dealer))
        {
            // PLAYER'S TURN
            // -- Allow to chose between available options with text input (prompted)
            if (PlayerTurn()) // if player doesn't bust, DEALER's TURN
                DealerTurn();

            // DECIDE THE WINNER
            // -- based on resultant hands held after user & dealer have both had their turn
            DecideWinner();
        }

        ResetCards();
        return (_user._currentMoney > 0);
    }

    public void InitialDraw()
    {
        _user._hand.AddCard(_deck);
        _user._hand.AddCard(_deck);
        _dealer._hand.AddCard(_deck);
        _dealer._hand.AddCard(_deck);
    }

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
                    break;
                case 2:
                    // STAND
                    _ui.PlayerAction_ChoiceMessage("Stand");
                    // Console.WriteLine($"It is now the Dealer's turn.");
                    keepDrawing = false;
                    break;
                case 3:
                    // DOUBLE DOWN
                    _ui.PlayerAction_ChoiceMessage("Double Down");
                    _ui.PlayerAction_NotSupportedMessage(); // TO-DO
                    keepDrawing = false;
                    break;
                case 4:
                    // SPLIT
                    _ui.PlayerAction_ChoiceMessage("Split");
                    _ui.PlayerAction_NotSupportedMessage(); // TO-DO
                    break;
                case 5:
                    // SURRENDER 
                    _ui.PlayerAction_ChoiceMessage("Surrender");
                    _ui.PlayerAction_NotSupportedMessage(); // TO-DO
                    break;
                default:
                    // unknown input
                    Console.WriteLine("Your input was not an integer in the correct range (1-5). Please try again.");
                    break;
            }
        }
        Console.WriteLine();
        return !busted;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool PlayerAction_Hit()
    {
        _user._hand.AddCard(_deck);
        _ui.PlayerAction_HitMessage(_user);
        _ui.DisplayHands(_user, _dealer, hideDealersFirstCard: true);
        return (!GameRules.CheckForBlackjack(_user._hand) && !GameRules.CheckForBust(_user._hand));
    }

    public bool PlayerAction_DoubleDown() // TO-DO: complete this implementation
    {
        IncrementBet(raise: _user._hand._betAmount);
        bool busted = PlayerAction_Hit();
        return false; // TO-DO: finish
    }

    /// <summary>
    /// 
    /// </summary>
    private void DealerTurn()
    {
        // Reveal Hidden Card
        _dealer._doHideFirstCard = false;
        _ui.DisplayHands(_user, _dealer, hideDealersFirstCard: _dealer._doHideFirstCard);
        Thread.Sleep(5000);

        while (_dealer._hand._currentScore < 17)
        {
            Console.Clear();
            // Draw a card from the deck and add it to the Dealer's hand
            _dealer._hand.AddCard(_deck);
            // Retrieve the card just added & display a message describing this action
            _ui.CardDrawnMessage(_dealer);
            Thread.Sleep(500);
            // Update the display representing players' hands
            _ui.DisplayHands(_user, _dealer, hideDealersFirstCard: _dealer._doHideFirstCard);
            Thread.Sleep(2000);
        }

        // Check for bust
        if (_dealer._hand._currentScore > 21)
        {
            Console.WriteLine("The Dealer BUST! That means ...");
        }
    }

    /// <summary>
    /// Creates a new shuffled deck and empties the hands by reassigning them to new Hand objects. 
    /// </summary>
    public void ResetCards()
    {
        _deck = new Deck(doShuffle: true);
        _user._hand = new Hand(betAmount: -1, isDealer: false);
        _dealer._hand = new Hand(betAmount: -1, isDealer: true);
    }

    /// <summary>
    /// 
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
                else if (betAmount <= GameRules.MINIMUM_BET)
                    _ui.PromptAfterError($"The minimum bet is {GameRules.MINIMUM_BET:C0}", isBet: true);
                else 
                    validBet = SetBet(betAmount); // SUCCESS!
            }
        }
    }

    public bool SetBet(int newBet)
    {
        if (newBet > _user._currentMoney)
            return false;
        else
        {
            _user._hand._betAmount = newBet;
            return true;
        }
    }
    public bool IncrementBet(int raise)
    {
        if (_user._hand._betAmount + raise > _user._currentMoney)
            return false;
        else
        {
            _user._hand._betAmount += raise;
            return true;
        }
    }

    public void DecideWinner()
    {
        bool userBusted = (_user._hand._currentScore > 21);
        bool dealerBusted = (_dealer._hand._currentScore > 21);

        // for(int i = 0; i < _user._hands.Count; i++)
        if (userBusted || (_user._hand._currentScore < _dealer._hand._currentScore && !dealerBusted)) // DEALER wins (USER loses)
        {
            GameRules.ResultLose(_ui, _user);
        }
        else if (dealerBusted || _user._hand._currentScore > _dealer._hand._currentScore) // USER wins!!
        {
            GameRules.ResultWin(_ui, _user);
        }
        else // TIED
        {
            GameRules.ResultTie(_ui, _user);
        }

        _ui.PromptToContinue();
    }

}