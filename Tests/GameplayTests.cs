using BlackjackGame.Models;
using BlackjackGame.Core;
using BlackjackGame.UI;

namespace BlackjackGame.Tests;

public static class GameplayTests {

    public static bool Execute(string[] args)
    {
        bool testSuccessful;

        if (args.Length == 1 && int.TryParse(args[0], out int debugChoice))
        {
            switch (debugChoice)
            {
                case 1:
                    testSuccessful = TEST_NaturalBlackjack();
                    break;
                case 2:
                    testSuccessful = TEST_PlayerBusts();
                    break;
                case 3:
                    testSuccessful = TEST_AceReducesToOne();
                    break;
                default:
                    testSuccessful = false;
                    Console.WriteLine("Test request not recognized");
                    break;
            }
            return testSuccessful;
        }
        else
        {
            // Console.Clear();
            Console.WriteLine("ERROR: Multiple arguments is not supported as input.\n\n\n\n");
            throw new ArgumentException();
        }
    }

    private static bool TEST_NaturalBlackjack()
    {
        // Arrange: Stack the deck so the user gets a natural blackjack (Ace + 10-value card), dealer gets non-blackjack
        var stackedCards = new List<Card>
            {
                new Card("Spades", 1),    // Player 1 (user) - Ace
                new Card("Diamonds", 9),  // Dealer 1
                new Card("Clubs", 10),    // Player 2 (user) - 10
                new Card("Hearts", 7)     // Dealer 2
            };
        Deck testDeck = new Deck(stackedCards);
        User testUser = new User(100); // Give user enough money
        Dealer testDealer = new Dealer();
        testUser._hand = new Hand(betAmount: 10, isDealer: false);
        testDealer._hand = new Hand(betAmount: 0, isDealer: true);

        // Use the existing UI_TextBased (or UI_ASCII) for testing
        IGameUI testUI = new UI_TextBased();

        // Act: Run the game logic for a single round
        GameEngine engine = new GameEngine(testUI, testDeck, testUser, testDealer);
        bool canContinue = engine.StartGame();

        // Assert: User should have blackjack, dealer should not, and winnings should be correct
        bool userHasBlackjack = GameRules.CheckForBlackjack(testUser._hand);
        bool dealerHasBlackjack = GameRules.CheckForBlackjack(testDealer._hand);
        float expectedMoney = 100 - 10 + (10 * GameRules.WIN_RATIO_NATURAL_BLACKJACK) + 10; // bet returned + 1.5x winnings
        bool winningsCorrect = Math.Abs(testUser._currentMoney - expectedMoney) < 0.01f;

        Console.WriteLine($"User hand: {testUser._hand} (score: {testUser._hand._currentScore})");
        Console.WriteLine($"Dealer hand: {testDealer._hand} (score: {testDealer._hand._currentScore})");
        Console.WriteLine($"User has blackjack: {userHasBlackjack}");
        Console.WriteLine($"Dealer has blackjack: {dealerHasBlackjack}");
        Console.WriteLine($"User money after win: {testUser._currentMoney} (expected: {expectedMoney})");
        Console.WriteLine($"Winnings correct: {winningsCorrect}");

        return userHasBlackjack && !dealerHasBlackjack && winningsCorrect;
    }

    private static bool TEST_PlayerBusts()
    {
        // Arrange: Use Card constructor (string suit, int value)
        var stackedCards = new List<Card>
        {
            new Card("Spades", 10),    // Player 1
            new Card("Diamonds", 9),   // Dealer 1
            new Card("Clubs", 7),      // Player 2
            new Card("Hearts", 7),     // Dealer 2
            new Card("Spades", 5),     // Player hits and busts (10+7+5=22)
        };
        Deck testDeck = new Deck(stackedCards);
        User testUser = new User(100);
        Dealer testDealer = new Dealer();
        testUser._hand = new Hand(betAmount: 10, isDealer: false);
        testDealer._hand = new Hand(betAmount: 0, isDealer: true);

        // Act: Deal initial cards
        testUser._hand.AddCard(testDeck);
        testUser._hand.AddCard(testDeck);
        testDealer._hand.AddCard(testDeck);
        testDealer._hand.AddCard(testDeck);
        // Player hits
        testUser._hand.AddCard(testDeck);

        // Calculate scores (if not done automatically)
        testUser._hand._currentScore = GameRules.CalculateHandValue(testUser._hand);
        testDealer._hand._currentScore = GameRules.CalculateHandValue(testDealer._hand);

        // Assert: User should bust
        bool userBusted = GameRules.CheckForBust(testUser._hand);
        Console.WriteLine($"User hand: {testUser._hand} (score: {testUser._hand._currentScore})");
        Console.WriteLine($"Dealer hand: {testDealer._hand} (score: {testDealer._hand._currentScore})");
        Console.WriteLine($"User busted: {userBusted}");
        return userBusted;
    }

    private static bool TEST_AceReducesToOne()
    {
        // Arrange: Stack the deck so the user gets Ace, 9, and then 5 (total would be 25 if Ace=11, but should be 15 if Ace=1)
        var stackedCards = new List<Card>
        {
            new Card("Spades", 1),    // Player 1 (user) - Ace
            new Card("Diamonds", 9),  // Dealer 1
            new Card("Clubs", 9),     // Player 2 (user) - 9
            new Card("Hearts", 7),    // Dealer 2
            new Card("Spades", 5)     // Player hits - 5
        };
        Deck testDeck = new Deck(stackedCards);
        User testUser = new User(100);
        Dealer testDealer = new Dealer();
        testUser._hand = new Hand(betAmount: 10, isDealer: false);
        testDealer._hand = new Hand(betAmount: 0, isDealer: true);
        IGameUI testUI = new UI_TextBased();

        // Act: Deal initial cards
        testUser._hand.AddCard(testDeck); // Ace
        testUser._hand.AddCard(testDeck); // 9
        testDealer._hand.AddCard(testDeck);
        testDealer._hand.AddCard(testDeck);
        testUser._hand.AddCard(testDeck); // 5

        // Calculate score (should be 1+9+5=15, not 11+9+5=25)
        testUser._hand._currentScore = GameRules.CalculateHandValue(testUser._hand);

        // Assert: User should NOT bust, and score should be 15
        bool userBusted = GameRules.CheckForBust(testUser._hand);
        bool correctScore = testUser._hand._currentScore == 15;
        Console.WriteLine($"User hand: {testUser._hand} (score: {testUser._hand._currentScore})");
        Console.WriteLine($"User busted: {userBusted}");
        Console.WriteLine($"Ace reduced to 1: {correctScore}");
        return !userBusted && correctScore;
    }
}