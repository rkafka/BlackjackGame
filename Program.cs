﻿using BlackjackGame.Models;
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
        Players can continue hitting until they stand or bust (go over 21).

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

// user._hand.AddCard(deck);
// user._hand.AddCard(deck);
// dealer._hand.AddCard(deck);
// dealer._hand.AddCard(deck);
game.InitialDraw();


// Console.ForegroundColor = ConsoleColor.Blue;
// Console.Write(""); // TO-DO: WRITE HEADER for USER

// Console.ForegroundColor = ConsoleColor.White;
int x, y;
(x, y) = Console.GetCursorPosition();
Startup.BootSequence();
Console.Clear();
Startup.PrintTitle();
game.UI_Hands();


// string cardToPrint = user._hand.cardList[0].GetASCII();
// // Console.WriteLine(user._hand.cardList[0].GetASCII());
// ASCII.DisplayASCII(cardToPrint);

// x += cardToPrint.Split("\n")[0].Length;
// y -= cardToPrint.Split("\n").Length;
// Console.SetCursorPosition(x, y);
// ASCII.DisplayASCII(cardToPrint);
// Console.WriteLine(user._hand.cardList[1].GetASCII());

// deck.Print();

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