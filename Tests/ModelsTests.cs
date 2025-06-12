namespace BlackjackGame.Models;

public class ModelsTests
{
    public static void Execute(string[] args)
    {
        test_Hand_Deck();
    }

    static void test_Hand_Deck()
    {
        // Hand hand = new();

        Hand.printDeck();
        Console.WriteLine();

        Hand.shuffleDeck();
        Hand.printDeck();
    }
}

