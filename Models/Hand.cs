namespace BlackjackGame.Models;

public class Hand
{
    public static List<Card> deck = createDeck();


    public int currentScore;
    public List<Card> cardList;
    public bool isDealer;

    public Hand(bool isDealer=false)
    {
        this.isDealer = isDealer;

        cardList = [];
        // initial deal
        addCard();
        addCard();
    }

    public void addCard()
    {
        Random rng = new();
        Card newCard = deck[rng.Next(Card.MinAllowedRank, Card.MaxAllowedRank)];
        this.cardList.Add(newCard);
    }

    public static List<Card> createDeck(bool doShuffle=true)
    {
        List<Card> newDeck = [];
        foreach (string suitValue in Card.suitDict.Keys)
        {
            for (int rankValue = Card.MinAllowedRank; rankValue <= Card.MaxAllowedRank; rankValue++)
            {
                newDeck.Add(new Card(suitValue, rankValue));
            }
        }
        return newDeck;
    }

    public static void shuffleDeck()
    {
        if (deck.Count < 1)
            throw new Exception("Attempted to shuffle a non-existent deck.");

        // PERFORM A FISHER YATES SHUFFLE
        Random rng = new();
        for (int iterativeIndex = 0; iterativeIndex < deck.Count; iterativeIndex++)
        {
            int randomIndex = rng.Next(0, deck.Count);
            (deck[iterativeIndex], deck[randomIndex]) = (deck[randomIndex], deck[iterativeIndex]);
            iterativeIndex++;
        }
    }

    public static void printDeck(int cardsPerLine = 4)
    {
        // TO-DO: add bounds for cardsPerLine (base on console window width?)
        int longestCardNameLength = "Queen of Diamonds".Length;

        Console.Write("\nCurrent Deck:  \n| ");
        for (int i = 0; i < deck.Count; i++)
        {
            Console.Write($"({(i+1).ToString().PadRight(2)}) {deck[i].ToString().PadRight((longestCardNameLength), ' ')} | ");
            if (i % cardsPerLine == cardsPerLine - 1)
                Console.Write("\n| ");
        }
        Console.WriteLine();
    }
}