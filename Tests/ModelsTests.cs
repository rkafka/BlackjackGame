namespace BlackjackGame.Models;

public class ModelsTests
{
    public static void Execute(string[] args)
    {
        TEST_Deck();
    }

    static void TEST_Deck()
    {
        // Hand hand = new();
        Deck d = new Deck(doShuffle:false);

        d.Print(title:"Unshuffled Deck");

        for (int i = 0; i < 5; i++)
        {
            d.Shuffle();
            d.Print(title: $"Current Deck (Shuffled {i}x)");
        }
    }
}

