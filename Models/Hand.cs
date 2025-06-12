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
            for (int rankValue = 1; rankValue < Card.numberOfRanks; rankValue++)
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
        int iterativeIndex = 0; // index iterating through each card in the deck sequentially
        while (iterativeIndex < deck.Count)
        {
            int randomIndex = rng.Next(deck.Count);
            (deck[iterativeIndex], deck[randomIndex]) = (deck[randomIndex], deck[iterativeIndex]);
            iterativeIndex--;
        }
    }

    public static void printDeck(int cardsPerLine=5)
    {
        // TO-DO: add bounds for cardsPerLine (base on console window width?)

        Console.WriteLine("Current Deck:");
        for (int i = 0; i < deck.Count; i++)
        {
            Console.Write(deck[i] + ",   ");
            if (i % cardsPerLine == cardsPerLine-1)
                Console.Write("\n");
        }       
    }
}