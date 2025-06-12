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

        Hand.printDeck(title:"Unshuffled Deck");

        for (int i = 0; i < 5; i++)
        {
            Hand.shuffleDeck();
            Hand.printDeck(title: $"Current Deck (Shuffled {i}x)");
        }
    }
}

