using BlackjackGame.Models;
using BlackjackGame.UI;
using BlackjackGame.Utils;
using BlackjackGame.Core;
using BlackjackGame.Tests;


// Ensure clean execution
Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.White;
Console.Clear();

// TO-DO: start menu

if (args.Length >= 2)
{
    if (args[0].ToLower().Equals("debug"))
    {
        switch (args[1].ToLower())
        {
            case "gameplay":
                GameplayTests.Execute(args[2..]);
                break;
            case "models":
                ModelsTests.Execute(args[2..]);
                break;
            case "menu":
                MenuTests.Execute(args[2..]);
                break;
            default:
                Console.WriteLine("ERROR:  can not find the desired debug module.\n\n\n\n");
                break;
        }
        return; // end execution after debug
    }
}

// new
IGameUI ui;
if (args.Length > 0 && args[0] == "ascii") { ui = new UI_ASCII(); }
else { ui = new UI_TextBased(); }

BJGame game = new BJGame(ui);
game.Run();


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