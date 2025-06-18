using BlackjackGame.Models;
using BlackjackGame.UI;
using BlackjackGame.Utils;



Startup.PrintTitle();
if(args.Length > 0 && args[0] == "debug")
    ModelsTests.Execute(args);


// GAMEPLAY SEQUENCE
/*
    1. Initial Deal
        - Dealer gives 2 cards to each player (face up)
        - Dealer gets 2 cards: one face up, one face down (the “hole” card)

    2. Blackjack Check
        - If a player or dealer has an Ace + 10-value card → it’s a Blackjack
        - Blackjack pays 3:2 (e.g., bet $10, win $15)
        - If both player and dealer have Blackjack → Push (tie)

    3. Player Actions
        - For each hand, players choose:
            Hit – take another card
            Stand – keep current hand
            Double Down – double your bet, take one card only
            Split – if you have a pair, split into 2 hands (each gets another card)
            Surrender – (if allowed) forfeit half your bet and end the hand
        - Players can continue hitting until they stand or bust (go over 21).

    4. Dealer’s Turn
        - Dealer reveals the face-down card
        - Dealer must hit until total is 17 or higher
            - Most casinos force dealer to stand on soft 17 (A+6)
            - Dealer busts if over 21
*/


/// 1. Initial Deal
///     - Dealer gives 2 cards to each player (face up)
///     - Dealer gets 2 cards: one face up, one face down (the “hole” card)
Deck deck = new Deck(doShuffle:true);
deck.Print();

User user = new();
Dealer dealer = new();
Game game = new(deck, user, dealer);


// Console.ForegroundColor = ConsoleColor.White;
int x, y;
(x, y) = Console.GetCursorPosition();
Startup.BootSequence();
Console.Clear();

bool gameOver = false;
while (!gameOver)
{
    // Initial Deal
    Startup.PrintTitle();

    game.SetBet();

    game.InitialDraw();


    game.PrintAllHandsAsText();
    // game.UI_Hands();
    // Blackjack Check
    int roundStatus = game.BlackjackCheck();
    bool roundOver = false;
    switch (roundStatus)
    {
        case 0:
            // nobody wins, continue
            if (game.PlayerActions() != 0)
            {
                // Dealer's turn
                game.DealersTurn();
            }
            game.DecideWinner();
            if (game._user._currentMoney <= 0)
                gameOver = true;
            else
                game.ResetCards();
            break;
        case 1:
            // user wins! :) end round
            roundOver = true;
            game.resultWin();
            game.ResetCards();
            continue;
        case 2:
            // dealer wins... :( end round
            roundOver = true;
            game.resultLose();
            if (game._user._currentMoney <= 0)
                gameOver = true;
            else
                game.ResetCards();
            continue;
        case 3:
            // BOTH got blackjack
            // end round, no winner
            roundOver = true;
            game.resultTie();
            game.ResetCards();
            continue;
        default:
            throw new ArgumentOutOfRangeException(nameof(roundStatus), "returned invalid value from BlackjackCheck(). Accepted values are 0, 1, 2, and 3");
    }

    // // Player Actions
    // int returnCode_PlayerActions = game.PlayerActions();
    

    // if (game._user._currentMoney <= 0)
    // {
    //     gameOver = true;
    // }
    // else //TO-DO: reset the hands (and deck?)
    // {
    //     game.ResetCards();
    // }
}
game.GameOver();

// WIN CONDITIONS
/* 
    Player busts    ------->    Player loses
    Dealer busts	------->    Player wins
    Player > Dealer (≤21) ->    Player wins
    Dealer > Player (≤21) ->    Player loses
    Player == Dealer  ----->    Push (tie, no win or loss)
*/

// PAYOUTS
/*
    Regular win:    1:1 (bet $10, win $10)
    Blackjack:      3:2 (bet $10, win $15)
    Insurance:      2:1 (side bet if dealer shows an Ace — not recommended!)
*/



// new
IGameUI ui;
if (args.Length > 0 && args[0] == "ascii") { ui = new UI_ASCII(); }
else { ui = new UI_TextBased(); }
BlackjackGame game = new BlackjackGame(ui);
game.Run();
